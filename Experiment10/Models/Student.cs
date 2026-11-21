using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPI.Models
{
    /// <summary>
    /// Represents a student entity in the Student Management System.
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Primary Key (Auto-Generated Identity).
        /// </summary>
        [Key]
        public int StudentId { get; set; }

        /// <summary>
        /// Alias for reference project compatibility.
        /// </summary>
        public int Id
        {
            get => StudentId;
            set => StudentId = value;
        }

        /// <summary>
        /// Full Name of the student.
        /// </summary>
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full Name must be between 2 and 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Alias for reference project compatibility.
        /// </summary>
        public string Name
        {
            get => FullName;
            set => FullName = value;
        }

        /// <summary>
        /// Valid email address of the student.
        /// </summary>
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email address cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number of the student.
        /// </summary>
        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone Number cannot exceed 20 characters.")]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Academic department or branch of study.
        /// </summary>
        [Required(ErrorMessage = "Department is required.")]
        [StringLength(50, ErrorMessage = "Department cannot exceed 50 characters.")]
        public string Department { get; set; } = string.Empty;

        /// <summary>
        /// Current academic semester (1 to 8).
        /// </summary>
        [Required(ErrorMessage = "Semester is required.")]
        [Range(1, 8, ErrorMessage = "Semester must be an integer between 1 and 8.")]
        public int Semester { get; set; }

        /// <summary>
        /// Cumulative Grade Point Average (0.00 to 10.00).
        /// </summary>
        [Required(ErrorMessage = "CGPA is required.")]
        [Range(0.00, 10.00, ErrorMessage = "CGPA must be between 0.00 and 10.00.")]
        public decimal CGPA { get; set; }

        /// <summary>
        /// Residential mailing address.
        /// </summary>
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string? Address { get; set; }

        /// <summary>
        /// Record creation timestamp (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
