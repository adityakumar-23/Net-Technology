using Experiment7.Models;

namespace Experiment7.Services
{
    public interface IFeedbackRepository
    {
        IEnumerable<FeedbackModel> GetAll();
        FeedbackModel? GetById(int id);
        FeedbackModel Add(FeedbackModel feedback);
        bool UpdateStatus(int id, FeedbackStatus status, string? notes = null);
        bool Delete(int id);
        FeedbackSummaryViewModel GetSummary();
        IEnumerable<FeedbackModel> Filter(string? searchTerm, string? category, int? rating, FeedbackStatus? status);
    }
}
