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

        [HttpGet]
        public IActionResult UserCert(string id) {
            var result = _db.Results.Where(x => x.UserId == id).Include(x => x.Certification).ToList();
            return View(result);
        }

        [HttpGet]
        public ActionResult Index(string id) {
            var answers = _db.Answers.Where(x => x.ResultId == id).Include(x => x.Question).Include(x => x.Result)
                .ThenInclude(x => x.Certification).ToList();
            return View(answers);
        }

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