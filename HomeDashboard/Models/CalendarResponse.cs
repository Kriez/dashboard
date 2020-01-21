using HomeDashboard.Library.Models.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeDashboard.Models
{
    public class CalendarResponse
    {
        public CalendarResponse()
        {
            this.Calendars = new List<CalendarItemDto>();
        }

        public DateTime LastUpdated { get; set; }
        public List<CalendarItemDto> Calendars { get; set; }
    }
}
