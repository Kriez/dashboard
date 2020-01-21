using HomeDashboard.HueWorkerService.Work;
using HomeDashboard.Library.Models;
using kriez.HomeDashboard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HueWorkerService
{
    public class HueService : IHostedService, IDisposable
    {
        private static object _locker = new object();
        private readonly ILogger<HueService> _logger;
        private readonly TimeSpan TimerInterval;
        private readonly bool IsActivated = false;
        private readonly IServiceScopeFactory _scopeService;

        private string _ipAdress;
        private readonly string _appKey;
        private Timer _timer;        

        public HueService(IConfiguration configuration, ILogger<HueService> logger, IServiceScopeFactory scopeService)
        {
            var seconds = Int32.Parse(configuration.GetSection("HostedServices").GetSection("HueService").GetSection("UpdateIntervalSeconds").Value);

            if (seconds > 0)
            {
                TimerInterval = new TimeSpan(0, 0, seconds);
                IsActivated = true;
            }

            _appKey = configuration.GetSection("HostedServices").GetSection("HueService").GetSection("appkey").Value;

            _logger = logger;
            _scopeService = scopeService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            IBridgeLocator locator = new HttpBridgeLocator();
            var bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
            _ipAdress = bridgeIPs.First().IpAddress;

            if (IsActivated)
            {
                _logger.LogInformation($"HueService running. Update interval set to {TimerInterval.TotalSeconds} seconds.");
                _timer = new Timer(DoWork, null, TimerInterval, TimerInterval);
            }
            else
            {
                _logger.LogInformation($"HueService is disabled");

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
                    _logger.LogInformation("HueService is doing some work.");

                    Worker worker = new Worker(context, _ipAdress, _appKey);
                    worker.ScanLightsAsync().Wait();
                    worker.ScanGroupsAsync().Wait();
                    worker.SaveAsync().Wait();

                    _logger.LogInformation("HueService is done with some work.");

                    if (context.ChangeTracker.HasChanges())
                    {
                        var update = context.UpdateTables.Single(u => u.Key.Equals(UpdateType.Hue));
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
