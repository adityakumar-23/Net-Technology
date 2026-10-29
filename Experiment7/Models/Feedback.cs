using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Experiment7.Models
{
    public enum FeedbackStatus
    {
        [Display(Name = "New")]
        New,
        [Display(Name = "Under Review")]
        UnderReview,
        [Display(Name = "Resolved")]
        Resolved,
        [Display(Name = "Archived")]
        Archived
    }

    public enum SentimentType
    {
        Positive,
        Neutral,
        Negative
    }

    public class FeedbackModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [Display(Name = "Full Name")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 60 characters.")]
        [RegularExpression(@"^[a-zA-Z\s\.\'-]+$", ErrorMessage = "Name can only contain alphabets, spaces, and hyphens.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Address is required.")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address (e.g., name@example.com).")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact Number is required.")]
        [Display(Name = "Contact Number")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter a valid 10-digit mobile number starting with 6-9.")]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a feedback category.")]
        [Display(Name = "Feedback Category")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Please provide a rating between 1 and 5 stars.")]
        [Display(Name = "Overall Satisfaction Rating")]
        public int Rating { get; set; } = 5;

        [Required(ErrorMessage = "Feedback comments are required.")]
        [Display(Name = "Detailed Feedback / Comments")]
        [StringLength(1000, MinimumLength = 15, ErrorMessage = "Feedback comments must be at least 15 characters long (max 1000).")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; } = string.Empty;

        [Display(Name = "I would recommend this institution / service to others")]
        public bool WouldRecommend { get; set; } = true;

        [Display(Name = "Submission Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Status")]
        public FeedbackStatus Status { get; set; } = FeedbackStatus.New;

        [Display(Name = "Admin / Reviewer Notes")]
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? AdminNotes { get; set; }

        // Helper property for sentiment based on rating
        public SentimentType Sentiment => Rating switch
        {
            >= 4 => SentimentType.Positive,
            3 => SentimentType.Neutral,
            _ => SentimentType.Negative
        };

        // Select list options for categories
        public static readonly List<string> StandardCategories = new()
        {
            "Course Curriculum & Content",
            "Teaching & Faculty Guidance",
            "Laboratory & Computing Facilities",
            "Library & Research Resources",
            "Campus Infrastructure & Cleanliness",
            "Administrative & Student Support"
        };

        public IEnumerable<SelectListItem> CategoryOptions =>
            StandardCategories.Select(c => new SelectListItem
            {
                Value = c,
                Text = c,
                Selected = (c == Category)
            });
    }

    public class FeedbackSummaryViewModel
    {
        public int TotalFeedbacks { get; set; }
        public double AverageRating { get; set; }
        public int PositiveCount { get; set; }
        public int NeutralCount { get; set; }
        public int NegativeCount { get; set; }
        public double RecommendationRate { get; set; }
        public int NewCount { get; set; }
        public int UnderReviewCount { get; set; }
        public int ResolvedCount { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
        public Dictionary<string, int> CategoryBreakdown { get; set; } = new();
    }

    public class FeedbackIndexViewModel
    {
        public IEnumerable<FeedbackModel> Feedbacks { get; set; } = new List<FeedbackModel>();
        public FeedbackSummaryViewModel Summary { get; set; } = new();
        public string? SearchTerm { get; set; }
        public string? SelectedCategory { get; set; }
        public int? SelectedRating { get; set; }
        public FeedbackStatus? SelectedStatus { get; set; }
        public string ViewMode { get; set; } = "grid"; // "grid" or "table"
        public IEnumerable<SelectListItem> CategoryFilterOptions { get; set; } = new List<SelectListItem>();
    }
}
