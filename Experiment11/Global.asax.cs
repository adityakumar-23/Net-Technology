using SmartCampusPortal.DAL;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartCampusPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Ensure SmartCampusDB and ServiceRequests table are initialized on LocalDB
            ServiceRequestDAL.EnsureDatabaseAndTable();
        }
    }
}
