using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OzSapkaTShirt.Data;
using OzSapkaTShirt.Models;

namespace OzSapkaTShirt2.Controllers
{
    public class InjectionController : Controller
    {
        public InjectionController(ApplicationContext applicationContext, UserManager<ApplicationUser> userManager)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
