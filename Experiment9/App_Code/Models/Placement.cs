using System;

namespace Experiment9.Models
{
    [Serializable]
    public class Placement
    {
        public int PlacementID { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string Department { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public DateTime OfferDate { get; set; }
        public decimal PackageOffered { get; set; }
        public string Designation { get; set; }
        public string Status { get; set; }

        public string FormattedPackage
        {
            get { return string.Format("₹{0:0.00} LPA", PackageOffered); }
        }

        public string FormattedDate
        {
            get { return OfferDate.ToString("dd MMM yyyy"); }
        }

        public string StatusBadgeClass
        {
            get
            {
                if (Status == "Accepted") return "badge bg-success";
                if (Status == "Pending") return "badge bg-warning text-dark";
                return "badge bg-danger";
            }
        }
    }
}
