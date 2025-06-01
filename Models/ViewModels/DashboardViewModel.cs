using System;
using System.Collections.Generic;
using dateManagementHTML.Models.Entities;

namespace dateManagementHTML.Models.ViewModels
{
    public class DashboardViewModel
    {
        public List<Event> TodayEvents { get; set; } = new List<Event>();
        public List<Reminder> TodayReminders { get; set; } = new List<Reminder>();

        public int CompletedEventCount { get; set; }
        public int TotalEventCount { get; set; }

        public int CompletionPercentage => TotalEventCount == 0 ? 0 : (int)((double)CompletedEventCount / TotalEventCount * 100);
    }
}
