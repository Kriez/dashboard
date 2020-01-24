using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDashboard.Library.Models.Calendar
{
    public class CalendarItemDto
    {
        public string Title { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Color { get; set; }
    }
}
