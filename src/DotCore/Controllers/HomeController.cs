using Microsoft.AspNetCore.Mvc;

namespace DotCore.Controllers
{
    public class HomeController : Controller
    {
        #region Methods

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        #endregion
    }
}