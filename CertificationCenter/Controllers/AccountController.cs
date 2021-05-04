using System;
using System.Threading.Tasks;
using CertificationCenter.Models;
using CertificationCenter.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CertificationCenter.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                User user = new User {UserName = model.UserName, Email = model.Email};
              
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "user");
                if (result.Succeeded) {
                    await _signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (ModelState.IsValid) {
                User user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null) {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
                    if (result.Succeeded) return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id) {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) {
                return NotFound();
            }

            EditUserViewModel model = new EditUserViewModel
                {Id = user.Id, UserName = user.UserName, Email = user.Email};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model) {
            if (ModelState.IsValid) {
                User user = await _userManager.FindByIdAsync(model.Id);

                var passwordValidator =
                    HttpContext.RequestServices.GetService(
                        typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                var passwordHasher =
                    HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                if (user != null) {
                    user.Email = model.Email;
                    user.UserName = model.UserName;

                    if (passwordValidator != null) {
                        IdentityResult result =
                            await passwordValidator.ValidateAsync(_userManager, user, model.Password);

                        if (result.Succeeded) {
                            if (passwordHasher != null)
                                user.PasswordHash = passwordHasher.HashPassword(user, model.Password);
                            await _userManager.UpdateAsync(user);
                            return RedirectToAction("Index", "Home");
                        }

                        foreach (var error in result.Errors) {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}