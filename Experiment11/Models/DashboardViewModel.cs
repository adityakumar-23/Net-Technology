using System.Collections.Generic;

namespace SmartCampusPortal.Models
{
    public class DashboardViewModel
    {
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int InProgressRequests { get; set; }
        public int CompletedRequests { get; set; }
        public int RejectedRequests { get; set; }

        public List<ServiceRequest> RecentRequests { get; set; }
        public Dictionary<string, int> CategoryCounts { get; set; }
        public Dictionary<string, int> StatusCounts { get; set; }

        public DashboardViewModel()
        {
            RecentRequests = new List<ServiceRequest>();
            CategoryCounts = new Dictionary<string, int>();
            StatusCounts = new Dictionary<string, int>();
        }
    }
}
