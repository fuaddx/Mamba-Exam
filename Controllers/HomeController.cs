using Mamba.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mamba.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}