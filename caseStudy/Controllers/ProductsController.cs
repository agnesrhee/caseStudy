using Microsoft.AspNetCore.Mvc;

namespace caseStudy.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
