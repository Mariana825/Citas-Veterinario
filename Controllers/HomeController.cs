using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PuppyCit.Models;
using System.Data;
using System.Diagnostics;

namespace PuppyCit.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
        public IActionResult Logout() //para el cierre de sesion
        {
            HttpContext.Session.Remove("User tel"); //remueve el telefono y usuario de la sesion
            return RedirectToAction("Index"); //lo redirige al home index
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
