using SmartCampusPortal.DAL;
using SmartCampusPortal.Models;
using System.Web.Mvc;

namespace SmartCampusPortal.Controllers
{
    public class ServiceRequestController : Controller
    {
        private readonly ServiceRequestDAL dal = new ServiceRequestDAL();

        // GET: ServiceRequest
        public ActionResult Index(string search = null, string category = null, string status = null)
        {
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentCategory = category ?? "All";
            ViewBag.CurrentStatus = status ?? "All";

            var requests = dal.GetAll(search, category, status);
            return View(requests);
        }

        // GET: ServiceRequest/Details/5
        public ActionResult Details(int id)
        {
            var request = dal.GetById(id);
            if (request == null)
            {
                return HttpNotFound("Service request not found.");
            }
            return View(request);
        }

        // GET: ServiceRequest/Create
        public ActionResult Create()
        {
            var model = new ServiceRequest();
            return View(model);
        }

        // POST: ServiceRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceRequest request)
        {
            if (ModelState.IsValid)
            {
                dal.Insert(request);
                TempData["SuccessMessage"] = "Service request #" + request.RequestId + " created successfully!";
                return RedirectToAction("Index");
            }

            return View(request);
        }

        // GET: ServiceRequest/Edit/5
        public ActionResult Edit(int id)
        {
            var request = dal.GetById(id);
            if (request == null)
            {
                return HttpNotFound("Service request not found.");
            }
            return View(request);
        }

        // POST: ServiceRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceRequest request)
        {
            if (ModelState.IsValid)
            {
                bool success = dal.Update(request);
                if (success)
                {
                    TempData["SuccessMessage"] = "Service request #" + request.RequestId + " updated successfully!";
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Unable to update the request. Please verify the ID.");
            }

            return View(request);
        }

        // GET: ServiceRequest/UpdateStatus/1
        public ActionResult UpdateStatus(int id)
        {
            var request = dal.GetById(id);
            if (request == null)
            {
                return HttpNotFound("Service request not found.");
            }

            ViewBag.RequestId = id;
            return View(request);
        }

        // POST: ServiceRequest/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int id, string status, string adminRemarks = null)
        {
            bool success = dal.UpdateStatus(id, status, adminRemarks);
            if (success)
            {
                TempData["SuccessMessage"] = "Status for request #" + id + " updated to '" + status + "' successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update status for request #" + id + ".";
            }

            return RedirectToAction("Index");
        }

        // GET: ServiceRequest/Delete/5
        public ActionResult Delete(int id)
        {
            var request = dal.GetById(id);
            if (request == null)
            {
                return HttpNotFound("Service request not found.");
            }
            return View(request);
        }

        // POST: ServiceRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bool success = dal.Delete(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Service request #" + id + " deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete service request #" + id + ".";
            }

            return RedirectToAction("Index");
        }
    }
}
