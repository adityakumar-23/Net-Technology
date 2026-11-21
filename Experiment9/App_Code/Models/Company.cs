using System;

namespace Experiment9.Models
{
    [Serializable]
    public class Company
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string JobRole { get; set; }
        public decimal PackageLPA { get; set; }
        public string EligibilityCriteria { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public string FormattedPackage
        {
            get { return string.Format("₹{0:0.00} LPA", PackageLPA); }
        }

        public string FormattedVisitDate
        {
            get { return VisitDate.ToString("dd MMM yyyy"); }
        }
    }
}
