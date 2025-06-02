using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Thamarat.Web.ViewModels;

namespace Thamarat.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // عرض كل المستخدمين
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var model = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserRolesViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles
                });
            }

            return View(model);
        }

        // GET: تعديل أدوار مستخدم
        public async Task<IActionResult> Manage(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                model.Add(new ManageUserRolesViewModel
                {
                    RoleName = role.Name,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }

            ViewBag.UserId = userId;
            ViewBag.Email = user.Email;
            return View(model);
        }

        // POST: تعديل أدوار مستخدم
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            var selectedRoles = model.Where(x => x.IsSelected).Select(y => y.RoleName);
            await _userManager.AddToRolesAsync(user, selectedRoles);

            return RedirectToAction("Index");
        }
    }
}
