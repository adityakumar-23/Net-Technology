using System;

namespace Experiment9.Models
{
    [Serializable]
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public int Semester { get; set; }
        public decimal CGPA { get; set; }
        public string Skills { get; set; }
        public string PlacementStatus { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool IsPlaced
        {
            get { return string.Equals(PlacementStatus, "Placed", StringComparison.OrdinalIgnoreCase); }
        }

        public string StatusBadgeClass
        {
            get { return IsPlaced ? "badge bg-success" : "badge bg-secondary"; }
        }
    }
}
