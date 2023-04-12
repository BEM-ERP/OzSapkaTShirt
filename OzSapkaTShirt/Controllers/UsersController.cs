using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OzSapkaTShirt.Data;
using OzSapkaTShirt.Models;

namespace OzSapkaTShirt.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _context;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return _userManager.Users != null ?
                        View(await _userManager.Users.Include(u => u.GenderType).Include(u => u.City).OrderBy(u => u.Name).ThenBy(u => u.SurName).ToListAsync()) :
                        Problem("Entity set 'ApplicationContext.Users'  is null.");
        }

        // GET: Userss/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            ApplicationUser? user;

            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            user = await _userManager.Users.Include(u => u.GenderType).Include(u => u.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            SelectList genders = new SelectList(_context.Genders, "Id", "Name");
            SelectList cities = new SelectList(_context.Cities, "PlateCode", "Name");

            ViewData["Genders"] = genders;
            ViewData["Cities"] = cities;
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SurName,Corporate,Address,Gender,BirthDate,UserName,Email,PhoneNumber,PassWord,ConfirmPassWord,CityCode")] ApplicationUser user)
        {
            IdentityResult? identityResult;
            SelectList genders, cities;

            if (ModelState.IsValid)
            {
                identityResult = _userManager.CreateAsync(user, user.PassWord).Result;
                if (identityResult == IdentityResult.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            genders = new SelectList(_context.Genders, "Id", "Name");
            cities = new SelectList(_context.Cities, "PlateCode", "Name");
            ViewData["Genders"] = genders;
            ViewData["Cities"] = cities;
            return View(user);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            SelectList genders, cities;

            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            genders = new SelectList(_context.Genders, "Id", "Name", user.Gender);
            cities = new SelectList(_context.Cities, "PlateCode", "Name", user.CityCode);
            ViewData["Genders"] = genders;
            ViewData["Cities"] = cities;
            return View(user.Trim());
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,SurName,Corporate,Address,Gender,BirthDate,UserName,Email,PhoneNumber,CityCode")] ApplicationUser user)
        {
            IdentityResult? identityResult;
            SelectList genders, cities;
            ApplicationUser existingUser;

            if (id != user.Id)
            {
                return NotFound();
            }

            ModelState["PassWord"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            ModelState["ConfirmPassWord"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            if (ModelState.IsValid)
            {
                existingUser = _userManager.FindByIdAsync(id).Result;
                existingUser.Name = user.Name;
                existingUser.SurName = user.SurName;
                existingUser.Corporate = user.Corporate;
                existingUser.Address = user.Address;
                existingUser.Gender = user.Gender;
                existingUser.BirthDate = user.BirthDate;
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.CityCode = user.CityCode;
                identityResult = _userManager.UpdateAsync(existingUser).Result;
                if (identityResult == IdentityResult.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            genders = new SelectList(_context.Genders, "Id", "Name", user.Gender);
            cities = new SelectList(_context.Cities, "PlateCode", "Name", user.CityCode);
            ViewData["Genders"] = genders;
            ViewData["Cities"] = cities;
            return View(user);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _userManager.Users == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.Include(u => u.GenderType).Include(u => u.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_userManager.Users == null)
            {
                return Problem("Entity set 'ApplicationContext.Product'  is null.");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return (_userManager.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
