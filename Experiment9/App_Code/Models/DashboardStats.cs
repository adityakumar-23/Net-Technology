using System;

namespace Experiment9.Models
{
    [Serializable]
    public class DashboardStats
    {
        public int TotalStudents { get; set; }
        public int TotalCompanies { get; set; }
        public int StudentsPlaced { get; set; }
        public int StudentsNotPlaced { get; set; }
        public int TotalPlacementOffers { get; set; }
        public decimal HighestPackageLPA { get; set; }
        public decimal AveragePackageLPA { get; set; }

        public double PlacementPercentage
        {
            get
            {
                if (TotalStudents > 0)
                {
                    return Math.Round(((double)StudentsPlaced / TotalStudents) * 100.0, 1);
                }
                return 0.0;
            }
        }
    }
}
