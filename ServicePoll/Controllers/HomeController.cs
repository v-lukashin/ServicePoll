using ServicePoll.Config;
using System.Web.Mvc;

namespace ServicePoll.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var str = ServicePollConfig.TempCollectionName;
            return View();
        }
    }
}
