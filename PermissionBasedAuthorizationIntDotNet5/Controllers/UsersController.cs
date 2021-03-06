using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PermissionBasedAuthorizationIntDotNet5.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace PermissionBasedAuthorizationIntDotNet5.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .Select(user => new UserViewModel { Id = user.Id, UserName = user.UserName, Email = user.Email, Roles = _userManager.GetRolesAsync(user).Result })
                .ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var roles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new CheckBoxViewModel
                {
                    DisplayValue = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.DisplayValue));

            //foreach (var role in model.Roles)
            //{
            //    if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
            //        await _userManager.RemoveFromRoleAsync(user, role.RoleName);

            //    if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
            //        await _userManager.AddToRoleAsync(user, role.RoleName);
            //}

            return RedirectToAction(nameof(Index));
        }
    }
}