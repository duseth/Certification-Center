using System;
using System.Threading.Tasks;
using CertificationCenter.Models;
using CertificationCenter.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CertificationCenter.Controllers {
    /// <summary>
    /// Контроллер управления аккаунтами.
    /// </summary>
    public class AccountController : Controller {
        /// <summary>
        /// Свойство предоставляющее постоянное хранилище объектам типа User и работу с ними.
        /// </summary>
        private readonly UserManager<User> _userManager;
        /// <summary>
        /// Свойство предоставляющее работу с авторизацией пользователей.
        /// </summary>
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Обработчик GET-запроса при обращении на страницу регистрации.
        /// </summary>
        /// <returns>Страницу регистрации.</returns>
        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        /// <summary>
        /// Обработчик POST-запроса на регистрацию пользователя.
        /// </summary>
        /// <param name="model">Модель представления для регистрации.</param>
        /// <returns>Перенаправление на домашнюю страницу при успешной регистрации,
        /// иначе страницу регистрации с ошибкой.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                User user = new User {UserName = model.UserName, Email = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    await _userManager.AddToRoleAsync(user, "user");
                    await _signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        
        /// <summary>
        /// Обработчик GET-запроса при обращении на страницу авторизации.
        /// </summary>
        /// <returns>Страницу авторизации.</returns>
        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        /// <summary>
        /// Обработчик POST-запроса на авторизацию пользователя.
        /// </summary>
        /// <param name="model">Модель представления для авторизации.</param>
        /// <returns>Перенаправление на домашнюю страницу при успешной авторизации,
        /// иначе страницу авторизации с ошибкой</returns>
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

        /// <summary>
        /// Обработчик GET-запроса при обращении на изменение пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Страницу для изменения данных о пользователе,
        /// если такого не существует - страницу с ошибкой.</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string id) {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null) {
                return NotFound();
            }

            EditAccountViewModel model = new EditAccountViewModel
                {Id = user.Id, UserName = user.UserName, Email = user.Email};
            return View(model);
        }

        /// <summary>
        /// Обработчик POST-запроса на изменение пользователя.
        /// </summary>
        /// <param name="model">Модель представления для изменения пользователя.</param>
        /// <returns>Перенаправление на домашнюю страницу при успешном изменении,
        /// иначе страницу с ошибкой</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(EditAccountViewModel model) {
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

                    if (passwordValidator != null && !string.IsNullOrEmpty(model.Password)) {
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

        /// <summary>
        /// Обработчик POST-запроса на выход из аккаунта.
        /// </summary>
        /// <returns>Перенаправление на страницу авторизации.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}