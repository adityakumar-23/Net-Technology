using SmartCampusPortal.DAL;
using System.Web.Mvc;

namespace SmartCampusPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ServiceRequestDAL dal = new ServiceRequestDAL();

        // GET: Home / Dashboard
        public ActionResult Index()
        {
            var stats = dal.GetDashboardStats();
            return View(stats);
        }
    }
}
