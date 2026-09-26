
using Microsoft.AspNetCore.Mvc;
using Experiment5.Models;

namespace Experiment5.Controllers
{
    public class LeaveController : Controller
    {
        private static List<LeaveRequest> leaves =
            new List<LeaveRequest>();

        public IActionResult Index()
        {
            return View(leaves);
        }

        [HttpGet]
        public IActionResult Apply()
        {
            string? studentName =
                Request.Cookies["StudentName"];

            ViewBag.StudentName = studentName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apply(LeaveRequest leave)
        {
            if (leave.FromDate > leave.ToDate)
            {
                ModelState.AddModelError(
                    "ToDate",
                    "To Date must be after From Date");

                return View(leave);
            }

            HttpContext.Session.SetString(
                "StudentName",
                leave.StudentName
            );

            Response.Cookies.Append(
                "StudentName",
                leave.StudentName,
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7),
                    HttpOnly = true
                }
            );

            leave.Id = leaves.Count + 1;
            leave.Status = "Pending";

            leaves.Add(leave);

            return RedirectToAction("Index");
        }
    }
}