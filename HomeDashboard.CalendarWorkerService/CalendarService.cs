using HomeDashboard.Library.Models;
using HomeDashboard.WorkerService.Calendar.Work;
using kriez.HomeDashboard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HomeDashboard.CalendarWorkerService
{
    public class CalendarService : IHostedService, IDisposable
    {
        private static object _locker = new object();
        private readonly ILogger<CalendarService> _logger;
        private readonly TimeSpan TimerInterval;
        private readonly bool IsActivated = false;
        private readonly IServiceScopeFactory _scopeService;

        private Timer _timer;

        public CalendarService(IConfiguration configuration, ILogger<CalendarService> logger, IServiceScopeFactory scopeService)
        {
            var seconds = 60 * 5;

            if (seconds > 0)
            {
                TimerInterval = new TimeSpan(0, 0, seconds);
                IsActivated = true;
            }


            _logger = logger;
            _scopeService = scopeService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (IsActivated)
            {
                _logger.LogInformation($"CalendarService running. Update interval set to {TimerInterval.TotalSeconds} seconds.");
                _timer = new Timer(DoWork, null, TimerInterval, TimerInterval);
            }
            else
            {
                _logger.LogInformation($"CalendarService is disabled");

            }
        }

        private void DoWork(object state)
        {
            var hasLock = false;

            try
            {
                Monitor.TryEnter(_locker, ref hasLock);
                if (!hasLock)
                {
                    return;
                }
                using (var scope = _scopeService.CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<DatabaseContext>();
                    context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                    _logger.LogInformation("CalendarService is doing some work.");

                    Worker worker = new Worker(context);
                    worker.ScanAsync();
                    worker.SaveAsync();

                    _logger.LogInformation("CalendarService is done with some work.");

                    if (context.ChangeTracker.HasChanges())
                    {
                        var update = context.UpdateTables.Single(u => u.Key.Equals(UpdateType.Calendar));
                        update.LastUpdated = DateTime.Now;
                        context.Entry(update).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HueService experienced some problems");

            }
            finally
            {
                if (hasLock)
                {
                    Monitor.Exit(_locker);
                    _timer.Change(TimerInterval, TimerInterval);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("HueService is stopping.");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _logger.LogInformation("HueService is disposed.");
            _timer?.Dispose();
        }
    }
}
