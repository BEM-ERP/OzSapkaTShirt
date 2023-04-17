using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OzSapkaTShirt.Data;
using OzSapkaTShirt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace OzSapkaTShirt.Areas.Admin.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            return _userManager.Users != null ?
                        View(await _userManager.Users.Include(u => u.GenderType).Include(u => u.City).OrderBy(u => u.Name).ThenBy(u => u.SurName).ToListAsync()) :
                        Problem("Entity set 'ApplicationContext.Users'  is null.");
        }
    }
}
