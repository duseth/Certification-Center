using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CertificationCenter.Models;
using CertificationCenter.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CertificationCenter.Controllers {
    public class AnswerController : Controller {
        private readonly ApplicationContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AnswerController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            ApplicationContext db) {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        /// <summary>
        /// GET-запрос на получения списка аттестаций, пройденных пользователем
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <returns>Список пройденных аттестаций</returns>
        [HttpGet]
        public IActionResult UserCert(string id) {
            var result = _db.Results.Where(x => x.UserId == id).Include(x => x.Certification).ToList();
            return View(result);
        }


        /// <summary>
        /// GET-запрос на получение списка ответов в конкретной аттестации
        /// </summary>
        /// <param name="id">id аттестации</param>
        /// <returns>Список ответов</returns>
        [HttpGet]
        public ActionResult Index(string id) {
            var answers = _db.Answers.Where(x => x.ResultId == id).Include(x => x.Question).Include(x => x.Result)
                .ThenInclude(x => x.Certification).ToList();
            return View(answers);
        }

        /// <summary>
        /// GET-запрос на просмотр пользователей администратором
        /// </summary>
        /// <returns>Список всех пользователей с ролью "user"</returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> IndexAdmin() {
            List<User> users = new List<User>();
            foreach (var user in _userManager.Users.ToList()) {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("admin")) {
                    users.Add(user);
                }
            }

            return View(users);
        }


        /// <summary>
        /// GET-Запрос на просмотр результатов аттестации администратором
        /// </summary>
        /// <param name="id">id аттестации</param>
        /// <returns>Список ответов, данных при прохождении аттестации</returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult EditAnswer(string id) {
            var answers = _db.Answers.Where(x => x.ResultId == id).Include(x => x.Question).Include(x => x.Result)
                .ThenInclude(x => x.Certification).Include(x => x.Result.User).ToList();
            EditAnswerViewModel model = new EditAnswerViewModel {
                Id = id,
                Answers = answers
            };

            return View(model);
        }

        /// <summary>
        /// POST-запрос на редактирование ответов администратором
        /// </summary>
        /// <param name="model">Модель ответа</param>
        /// <returns>Изменяет оценку у пользователя. Возвращает к списку пользователей</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditAnswer(EditAnswerViewModel model) {
            var result = _db.Answers.Where(x => x.ResultId == model.Id).ToList();
            foreach (var answer in result) {
                foreach (var change in model.ChangeStringList) {
                    answer.IsCorrect = change == "Верно";

                    _db.Update(answer);
                }
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("IndexAdmin");
        }
    }
}