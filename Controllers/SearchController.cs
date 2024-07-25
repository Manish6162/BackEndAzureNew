using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
