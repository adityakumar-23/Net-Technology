using System;
using System.ComponentModel.DataAnnotations;

namespace SmartCampusPortal.Models
{
    public class ServiceRequest
    {
        [Display(Name = "Request ID")]
        public int RequestId { get; set; }

        [Required(ErrorMessage = "Student Name is required.")]
        [StringLength(100, ErrorMessage = "Student Name cannot exceed 100 characters.")]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Student Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [Display(Name = "Student Email")]
        public string StudentEmail { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [StringLength(50, ErrorMessage = "Department cannot exceed 50 characters.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Service Category is required.")]
        [Display(Name = "Service Category")]
        public string ServiceCategory { get; set; }

        // Backward-compatible alias for existing reference project code
        public string ServiceType
        {
            get { return ServiceCategory; }
            set { ServiceCategory = value; }
        }

        [Required(ErrorMessage = "Request Title is required.")]
        [StringLength(150, ErrorMessage = "Title cannot exceed 150 characters.")]
        [Display(Name = "Request Title")]
        public string RequestTitle { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        public string Priority { get; set; }

        public string Status { get; set; }

        [Display(Name = "Request Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime RequestDate { get; set; }

        [Display(Name = "Admin Remarks")]
        [StringLength(250, ErrorMessage = "Remarks cannot exceed 250 characters.")]
        public string AdminRemarks { get; set; }

        public ServiceRequest()
        {
            Priority = "Medium";
            Status = "Pending";
            RequestDate = DateTime.Now;
        }
    }
}
