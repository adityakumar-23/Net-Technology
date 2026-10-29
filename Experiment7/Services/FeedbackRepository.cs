using System.Collections.Concurrent;
using Experiment7.Models;

namespace Experiment7.Services
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly ConcurrentDictionary<int, FeedbackModel> _feedbacks = new();
        private int _nextId = 1;
        private readonly object _lock = new();

        public FeedbackRepository()
        {
            SeedInitialFeedbacks();
        }

        private void SeedInitialFeedbacks()
        {
            var seedData = new List<FeedbackModel>
            {
                new()
                {
                    FullName = "Aarav Sharma",
                    Email = "aarav.sharma@example.com",
                    ContactNumber = "9876543210",
                    Category = "Teaching & Faculty Guidance",
                    Rating = 5,
                    Comments = "The faculty members are exceptionally supportive and provide in-depth practical examples during lectures. Doubt clearing sessions are well structured.",
                    WouldRecommend = true,
                    CreatedAt = DateTime.Now.AddDays(-6).AddHours(3),
                    Status = FeedbackStatus.Resolved,
                    AdminNotes = "Acknowledged and shared with the Department Head during monthly review."
                },
                new()
                {
                    FullName = "Priya Nair",
                    Email = "priya.nair@example.com",
                    ContactNumber = "9823456789",
                    Category = "Laboratory & Computing Facilities",
                    Rating = 4,
                    Comments = "Computers and development environments in Lab 3 have been upgraded recently. However, having high-speed dual monitors would make coding assignments much faster.",
                    WouldRecommend = true,
                    CreatedAt = DateTime.Now.AddDays(-4).AddHours(5),
                    Status = FeedbackStatus.UnderReview,
                    AdminNotes = "Request forwarded to Lab Systems Administrator for hardware budgeting."
                },
                new()
                {
                    FullName = "Rohan Verma",
                    Email = "rohan.verma@example.com",
                    ContactNumber = "9711223344",
                    Category = "Library & Research Resources",
                    Rating = 5,
                    Comments = "Access to IEEE and ACM digital research repositories has been seamless. The silent reading room is very conducive to research paper writing.",
                    WouldRecommend = true,
                    CreatedAt = DateTime.Now.AddDays(-3).AddHours(2),
                    Status = FeedbackStatus.Resolved,
                    AdminNotes = "Noted. IEEE subscription renewal confirmed."
                },
                new()
                {
                    FullName = "Ananya Desai",
                    Email = "ananya.desai@example.com",
                    ContactNumber = "9654321098",
                    Category = "Campus Infrastructure & Cleanliness",
                    Rating = 3,
                    Comments = "Water dispensers on the 2nd floor require frequent maintenance checks. Cafeteria seating can get overcrowded during peak lunch hours.",
                    WouldRecommend = false,
                    CreatedAt = DateTime.Now.AddDays(-2).AddHours(6),
                    Status = FeedbackStatus.UnderReview,
                    AdminNotes = "Estate office scheduled sanitization and filter change."
                },
                new()
                {
                    FullName = "Vikram Sengupta",
                    Email = "vikram.s@example.com",
                    ContactNumber = "9123456780",
                    Category = "Course Curriculum & Content",
                    Rating = 5,
                    Comments = "The updated cloud computing and containerization module is aligned with industry expectations. Hand-on projects were very enriching.",
                    WouldRecommend = true,
                    CreatedAt = DateTime.Now.AddDays(-1).AddHours(4),
                    Status = FeedbackStatus.New,
                    AdminNotes = null
                },
                new()
                {
                    FullName = "Meera Joshi",
                    Email = "meera.joshi@example.com",
                    ContactNumber = "9988776655",
                    Category = "Administrative & Student Support",
                    Rating = 2,
                    Comments = "Processing of scholarship verification forms took longer than expected. An online tracking portal would improve transparency significantly.",
                    WouldRecommend = false,
                    CreatedAt = DateTime.Now.AddHours(-5),
                    Status = FeedbackStatus.New,
                    AdminNotes = null
                }
            };

            foreach (var item in seedData)
            {
                item.Id = _nextId++;
                _feedbacks.TryAdd(item.Id, item);
            }
        }

        public IEnumerable<FeedbackModel> GetAll()
        {
            return _feedbacks.Values.OrderByDescending(f => f.CreatedAt).ToList();
        }

        public FeedbackModel? GetById(int id)
        {
            _feedbacks.TryGetValue(id, out var feedback);
            return feedback;
        }

        public FeedbackModel Add(FeedbackModel feedback)
        {
            lock (_lock)
            {
                feedback.Id = _nextId++;
                feedback.CreatedAt = DateTime.Now;
                feedback.Status = FeedbackStatus.New;
                _feedbacks.TryAdd(feedback.Id, feedback);
                return feedback;
            }
        }

        public bool UpdateStatus(int id, FeedbackStatus status, string? notes = null)
        {
            if (_feedbacks.TryGetValue(id, out var feedback))
            {
                feedback.Status = status;
                if (!string.IsNullOrWhiteSpace(notes))
                {
                    feedback.AdminNotes = notes;
                }
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            return _feedbacks.TryRemove(id, out _);
        }

        public FeedbackSummaryViewModel GetSummary()
        {
            var list = _feedbacks.Values.ToList();
            var total = list.Count;

            var summary = new FeedbackSummaryViewModel
            {
                TotalFeedbacks = total,
                AverageRating = total > 0 ? Math.Round(list.Average(f => f.Rating), 1) : 0,
                PositiveCount = list.Count(f => f.Sentiment == SentimentType.Positive),
                NeutralCount = list.Count(f => f.Sentiment == SentimentType.Neutral),
                NegativeCount = list.Count(f => f.Sentiment == SentimentType.Negative),
                RecommendationRate = total > 0 ? Math.Round((double)list.Count(f => f.WouldRecommend) / total * 100, 1) : 0,
                NewCount = list.Count(f => f.Status == FeedbackStatus.New),
                UnderReviewCount = list.Count(f => f.Status == FeedbackStatus.UnderReview),
                ResolvedCount = list.Count(f => f.Status == FeedbackStatus.Resolved)
            };

            for (int r = 1; r <= 5; r++)
            {
                summary.RatingDistribution[r] = list.Count(f => f.Rating == r);
            }

            foreach (var category in FeedbackModel.StandardCategories)
            {
                summary.CategoryBreakdown[category] = list.Count(f => f.Category == category);
            }

            return summary;
        }

        public IEnumerable<FeedbackModel> Filter(string? searchTerm, string? category, int? rating, FeedbackStatus? status)
        {
            var query = _feedbacks.Values.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                query = query.Where(f =>
                    f.FullName.ToLower().Contains(term) ||
                    f.Email.ToLower().Contains(term) ||
                    f.Comments.ToLower().Contains(term) ||
                    f.Category.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(f => f.Category == category);
            }

            if (rating.HasValue && rating.Value > 0)
            {
                query = query.Where(f => f.Rating == rating.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(f => f.Status == status.Value);
            }

            return query.OrderByDescending(f => f.CreatedAt).ToList();
        }
    }
}
