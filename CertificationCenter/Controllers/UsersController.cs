using System.Linq;
using System.Threading.Tasks;
using CertificationCenter.Models;
using CertificationCenter.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CertificationCenter.Controllers {
    [Authorize(Roles = "admin")]
    public class UsersController : Controller {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index() {
            return View(_userManager.Users.ToList());
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model) {
            if (ModelState.IsValid) {
                User user = new User {Email = model.Email, UserName = model.UserName};
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, model.Role);
                if (result.Succeeded) {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(string id) {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.Contains("admin") ? "admin" : "user";

            EditUserViewModel model = new EditUserViewModel
                {Id = user.Id, UserName = user.UserName, Email = user.Email, Role = role};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model) {
            if (ModelState.IsValid) {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null) {
                    user.Email = model.Email;
                    user.UserName = model.UserName;

                    var roles = await _userManager.GetRolesAsync(user);
                    
                    if (model.Role == "user" && roles.Contains("admin")) {
                        await _userManager.RemoveFromRoleAsync(user, "admin");
                    }

                    if (model.Role == "admin" && !roles.Contains("admin")) {
                        await _userManager.AddToRoleAsync(user, "admin");
                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded) {
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors) {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id) {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null) {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }
    }
}