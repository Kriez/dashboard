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
        private List<CalendarItem> _calendarItems;

        public Worker(DatabaseContext context)
        {
            _context = context;
            _calendarItems = new List<CalendarItem>();
        }

        public void ScanAsync()
        {
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
            request.MaxResults = 10;
         //   var timeMax = DateTime.Now.AddDays(1);
        //    request.TimeMax = new DateTime(timeMax.Year, timeMax.Month, timeMax.Day, 23, 59, 59);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            if (events.Items != null)
            {
                foreach (var eventItem in events.Items)
                {

                    CalendarItem calendarItem = new CalendarItem()
                        {
                            Id = eventItem.Id,
                            Title = eventItem.Summary,
                            Start = eventItem.Start.DateTime,
                            End = eventItem.End.DateTime,
                            Updated = eventItem.Updated
                        };
                    _calendarItems.Add(calendarItem);                  
                }
            }
        }

        public void SaveAsync()
        {
            foreach (var calendarItem in _calendarItems)
            {
                var oldCalendarItem = _context.CalendarItems.Find(calendarItem.Id);
                if (oldCalendarItem == null)
                {
                    _context.CalendarItems.Add(calendarItem);
                }
                else if(!calendarItem.Updated.Equals(oldCalendarItem.Updated))
                {
                    oldCalendarItem.Title = calendarItem.Title;
                    oldCalendarItem.Start = calendarItem.Start;
                    oldCalendarItem.End = calendarItem.End;
                    oldCalendarItem.Updated = calendarItem.Updated;            
                }
            }

            foreach (var calendarItem in _context.CalendarItems.ToList())
            {
                if (_calendarItems.Any(o => o.Id.Equals(calendarItem.Id)))
                {
                    continue;
                }

                _context.CalendarItems.Remove(calendarItem);
            }

        }
    }
}
