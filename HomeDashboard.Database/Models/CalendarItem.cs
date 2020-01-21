using System;
using System.Collections.Generic;
using System.Text;

namespace kriez.HomeDashboard.Data.Models
{
    public class CalendarItem
    {
        public string Id { get; set; }
        public DateTime? Updated { get; set; }
        public string Title { get; set; } //Summary
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
