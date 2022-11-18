using Microsoft.AspNetCore.Mvc;

namespace eTrainingSolution.WebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/School/[action]")]
    public class SchoolController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
