using System;

namespace Experiment5.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        public string StudentName { get; set; } = "";

        public string LeaveType { get; set; } = "";

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Reason { get; set; } = "";

        public string Status { get; set; } = "Pending";
    }
}