using System.Linq;
using System.Threading.Tasks;
using CertificationCenter.Models;
using CertificationCenter.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CertificationCenter.Controllers {
    /// <summary>
    /// Контроллер для управления пользователями.
    /// </summary>
    [Authorize(Roles = "admin")]
    public class UsersController : Controller {
        /// <summary>
        /// Свойство предоставляющее постоянное хранилище объектам типа User и работу с ними.
        /// </summary>
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager) {
            _userManager = userManager;
        }

        /// <summary>
        /// Обработчик GET-запроса при обращении на страницу пользователей.
        /// </summary>
        /// <returns>Страницу с пользователями.</returns>
        [HttpGet]
        public IActionResult Index() {
            return View(_userManager.Users.ToList());
        }

        /// <summary>
        /// Обработчик GET-запроса при обращении к странице с созданием пользователя.
        /// </summary>
        /// <returns>Страницу для создания пользователя.</returns>
        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        /// <summary>
        /// Обработчик POST-запроса на создание пользователя.
        /// </summary>
        /// <param name="model">Модель представления для создания пользователя.</param>
        /// <returns>Перенаправление на страницу пользователей при успешном добавлении,
        /// иначе на страницу ошибки.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model) {
            if (ModelState.IsValid) {
                User user = new User {Email = model.Email, UserName = model.UserName};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Обработчик GET-запроса при обращении на страницу изменения пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Страницу для изменения пользователя.</returns>
        [HttpGet]
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

        /// <summary>
        /// Обработчик POST-запроса на изменение пользователя.
        /// </summary>
        /// <param name="model">Модель представления для изменения пользователя.</param>
        /// <returns>Перенаправление на страницу пользователей при успешном изменении,
        /// иначе на страницу ошибки.</returns>
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
                        await _userManager.AddToRoleAsync(user, "user");

                    }

                    if (model.Role == "admin" && !roles.Contains("admin")) {
                        await _userManager.RemoveFromRoleAsync(user, "user");
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

        /// <summary>
        /// Обработчик POST-запроса на удаление пользователя.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Перенаправление на страницу пользователей.</returns>
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