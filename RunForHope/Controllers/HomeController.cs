using Microsoft.AspNetCore.Mvc;

namespace RunForHope_️.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Nueva acción para Contacto
        public IActionResult Contacto()
        {
            return View();
        }

        // Nueva acción para Donaciones
        public IActionResult Donaciones()
        {
            return View();
        }
    }
}
