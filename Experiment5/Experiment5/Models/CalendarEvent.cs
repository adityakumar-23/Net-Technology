using System;

namespace Experiment5.Models
{
    public class CalendarEvent
    {
        public int Id { get; set; }

        public string EventName { get; set; } = "";

        public DateTime EventDate { get; set; }

        public string Description { get; set; } = "";
    }
}