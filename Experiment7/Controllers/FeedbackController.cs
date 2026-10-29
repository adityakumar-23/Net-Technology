using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Experiment7.Models;
using Experiment7.Services;

namespace Experiment7.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackRepository _repository;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(IFeedbackRepository repository, ILogger<FeedbackController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: /Feedback or /
        public IActionResult Index(string? searchTerm, string? category, int? rating, FeedbackStatus? status, string viewMode = "grid")
        {
            var filtered = _repository.Filter(searchTerm, category, rating, status);
            var summary = _repository.GetSummary();

            var categoryFilterOptions = new List<SelectListItem>
            {
                new() { Value = "", Text = "All Categories", Selected = string.IsNullOrEmpty(category) }
            };
            categoryFilterOptions.AddRange(FeedbackModel.StandardCategories.Select(c => new SelectListItem
            {
                Value = c,
                Text = c,
                Selected = (c == category)
            }));

            var viewModel = new FeedbackIndexViewModel
            {
                Feedbacks = filtered,
                Summary = summary,
                SearchTerm = searchTerm,
                SelectedCategory = category,
                SelectedRating = rating,
                SelectedStatus = status,
                ViewMode = string.Equals(viewMode, "table", StringComparison.OrdinalIgnoreCase) ? "table" : "grid",
                CategoryFilterOptions = categoryFilterOptions
            };

            return View(viewModel);
        }

        // GET: /Feedback/Create
        public IActionResult Create()
        {
            var model = new FeedbackModel
            {
                Rating = 5,
                WouldRecommend = true
            };
            return View(model);
        }

        // POST: /Feedback/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FeedbackModel model)
        {
            // Business rule checks or custom validations
            if (model.Rating < 1 || model.Rating > 5)
            {
                ModelState.AddModelError(nameof(model.Rating), "Rating must be between 1 and 5 stars.");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the highlighted validation errors and try again.";
                return View(model);
            }

            var created = _repository.Add(model);
            _logger.LogInformation("Feedback #{Id} submitted by {Name} ({Email}) with rating {Rating}",
                created.Id, created.FullName, created.Email, created.Rating);

            TempData["SuccessMessage"] = $"Thank you, {created.FullName}! Your feedback has been recorded successfully as Ticket #{created.Id:D4}.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Feedback/Details/5
        public IActionResult Details(int id)
        {
            var feedback = _repository.GetById(id);
            if (feedback == null)
            {
                TempData["ErrorMessage"] = $"Feedback item #{id} was not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(feedback);
        }

        // POST: /Feedback/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, FeedbackStatus status, string? adminNotes, string? returnUrl)
        {
            var success = _repository.UpdateStatus(id, status, adminNotes);
            if (success)
            {
                TempData["SuccessMessage"] = $"Ticket #{id:D4} status updated to '{status}'.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Unable to update ticket #{id:D4}.";
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: /Feedback/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, string? returnUrl)
        {
            var success = _repository.Delete(id);
            if (success)
            {
                TempData["InfoMessage"] = $"Feedback record #{id:D4} has been deleted.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Could not delete feedback #{id:D4}.";
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Feedback/SummaryPartial (AJAX / modular view)
        public IActionResult SummaryPartial()
        {
            var summary = _repository.GetSummary();
            return PartialView("_FeedbackSummary", summary);
        }
    }
}
