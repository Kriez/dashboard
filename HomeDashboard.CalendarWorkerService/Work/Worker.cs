using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using kriez.HomeDashboard.Data;
using kriez.HomeDashboard.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace HomeDashboard.WorkerService.Calendar.Work
{
    public class Worker
    {
        private readonly DatabaseContext _context;
        private readonly string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        private readonly string ApplicationName = "HomeDashboard";

        public Worker(DatabaseContext context)
        {
            _context = context;

            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            var timeMax = DateTime.Now.AddDays(1);
            request.TimeMax = new DateTime(timeMax.Year, timeMax.Month, timeMax.Day, 23, 59, 59);
            request.ShowDeleted = false;
            request.SingleEvents = true;            
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    var calendarItem = _context.CalendarItems.Find(eventItem.Id);
                    if (calendarItem == null)
                    {
                        calendarItem = new CalendarItem()
                        {
                            Id = eventItem.Id,
                            Title = eventItem.Summary,
                            Start = eventItem.Start.DateTime,
                            End = eventItem.End.DateTime,
                            Updated = eventItem.Updated
                        };
                        _context.CalendarItems.Add(calendarItem);
                    }
                    else
                    {
                        if (calendarItem.Updated != eventItem.Updated)
                        {
                            calendarItem.Title = eventItem.Summary;
                            calendarItem.Start = eventItem.Start.DateTime;
                            calendarItem.End = eventItem.End.DateTime;

                            _context.Entry(calendarItem).State = EntityState.Modified;
                        }
                    }
                }
            }
            else
            {
                _context.CalendarItems.RemoveRange(_context.CalendarItems.ToList());
            }
        }
    }
}
