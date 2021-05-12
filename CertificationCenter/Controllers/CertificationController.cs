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
    /// <summary>
    /// Контроллер для работы с аттестациями.
    /// </summary>
    [Authorize]
    public class CertificationController : Controller {
        /// <summary>
        /// Свойство предоставляющее постоянное хранилище объектам типа User и работу с ними.
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Свойство предоставляющее контекст для работы с базой данных.
        /// </summary>
        private readonly ApplicationContext _db;

        public CertificationController(UserManager<User> userManager, ApplicationContext db) {
            _userManager = userManager;
            _db = db;
        }

        /// <summary>
        /// Обработчик GET-запроса при обращении на страницу аттестаций.
        /// </summary>
        /// <returns>Список всех аттестаций для данного пользователя, или всех аттестаций для админа.</returns>
        [HttpGet]
        public async Task<IActionResult> Index() {
            User user = await _userManager.GetUserAsync(User);

            var roles = await _userManager.GetRolesAsync(user);
            List<Certification> certifications = _db.Certifications.ToList();
            if (roles.Contains("admin")) {
                return View(certifications);
            }

            List<string> userCertificationsList = _db.UserCertifications
                .Where(e => e.UserId == user.Id)
                .Select(e => e.CertificationId)
                .ToList();

            List<Certification> certificationsByUser = certifications
                .Where(e => userCertificationsList.Contains(e.Id))
                .ToList();

            return View(certificationsByUser);
        }

        /// <summary>
        /// Обработчик GET-запроса при обращении на страницу создания аттестации.
        /// </summary>
        /// <returns>Страницу для создания аттестации.</returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Create() {
            CreateCertificationViewModel viewModel=new CreateCertificationViewModel();
            var topic = _db.Topics.Select(x => x.Name).ToList();
            viewModel.TopicList = topic;
            return View(viewModel);
        }

        /// <summary>
        /// Обработчик POST-запроса на создание аттестации.
        /// </summary>
        /// <param name="model">Модель представления для создания аттестации.</param>
        /// <returns>Перенаправление на страницу аттестаций при успешном добавлении,
        /// иначе страницу с ошибками.</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateCertificationViewModel model) {
            if (ModelState.IsValid && model.DatetimeEnd != null)
            {

                var topic =  _db.Topics.Where(x => x.Name == model.Topic).FirstOrDefault();
                Certification certification = new Certification {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Description = model.Description,
                    DatetimeStart = DateTime.Today,
                    DatetimeEnd = (DateTime) model.DatetimeEnd,
                    TopicId = topic.Id,
                    Topic = topic,
                    IsActive = true
                };

                await _db.Certifications.AddAsync(certification);

                foreach (string userId in Request.Form["user"]) {
                    User user = await _userManager.FindByIdAsync(userId);
                    UserCertifications userCertifications = new UserCertifications {
                        UserId = userId,
                        CertificationId = certification.Id,
                        User = user,
                        Certification = certification
                    };

                    await _db.UserCertifications.AddAsync(userCertifications);
                }

                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Pass(string id)
        {
            Certification certification = await _db.Certifications.FindAsync(id);
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            var questions = _db.Questions.Where(x => x.TopicId == certification.TopicId)
                .Select(x => x.QuestionString).ToList();
            var answear = new List<string>();
            foreach (var item in questions)
            {
                answear.Add("");
            }
            PassCertificationViewModel passCertification = new PassCertificationViewModel()
            {
                IdUser = user.Id,
                IdCertification = certification.Id,
                Name = certification.Name,
                Questions = questions,
                Answer = answear
            };
            return View(passCertification);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Pass(PassCertificationViewModel model)
        {
            if (model.IdUser != null)
            {
                Certification certification = await _db.Certifications.FindAsync(model.IdCertification);
                Result result=new Result()
                {
                    Id=Guid.NewGuid().ToString(),
                    CertificationId = model.IdCertification,
                    UserId=model.IdUser
                };
                await _db.Results.AddAsync(result);

                var questions = _db.Questions.Where(x => x.TopicId == certification.TopicId).ToList();
                foreach (var quest in questions)
                {

                    foreach (var item in model.Answer)
                    {

                        Answer answer = new Answer()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ResultId = result.Id,
                            QuestionId = quest.Id,
                            AnswerString = item,
                            IsCorrect = item.ToLower() == quest.AnswerString.ToLower() ? true : false
                        };
                        await _db.Answers.AddAsync(answer);
                    }
                }

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        /// <summary>
        /// Обработчик GET-запроса при обращении к странице изменения аттестации.
        /// </summary>
        /// <param name="id">Идентификатор аттестации.</param>
        /// <returns>Страницу для изменения аттестации,
        /// если такой аттестации не существует - страницу ошибки.</returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id) {
            Certification certification = await _db.Certifications.FindAsync(id);
            _db.Certifications.Where(x=>x.Id==certification.Id).Include(x=>x.Topic).Load();
            if (certification == null) {
                return NotFound();
            }

            var topic = _db.Topics.Select(x => x.Name).Where(x => x != certification.Topic.Name).ToList();
            var users = _db.UserCertifications
                .Where(e => e.CertificationId == certification.Id)
                .Select(e => e.UserId)
                .ToArray();

            EditCertificationViewModel editCertificationViewModel = new EditCertificationViewModel {
                Id = id,
                Name = certification.Name,
                Description = certification.Description,
                DatetimeEnd = certification.DatetimeEnd,
                Users = users,
                TopicList = topic,
                Topic = certification.Topic.Name
            };

            return View(editCertificationViewModel);
        }

        /// <summary>
        /// Обработчик POST-запроса на изменение аттестации.
        /// </summary>
        /// <param name="model">Модель представления для изменения аттестации.</param>
        /// <returns>Перенаправление на страницу аттестаций при успешном изменении,
        /// иначе страницу с ошибкой.</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(EditCertificationViewModel model) {
            model.Users = Request.Form["user"];
            if (ModelState.IsValid && model.DatetimeEnd != null) {
                Certification certification = await _db.Certifications.FindAsync(model.Id);

                if (certification == null) {
                    return NotFound();
                }

                certification.Name = model.Name;
                certification.Description = model.Description;
                certification.DatetimeEnd = (DateTime) model.DatetimeEnd;
                certification.Topic = _db.Topics.Where(x => x.Name == model.Topic).FirstOrDefault();

                var usersByCertification = _db.UserCertifications
                    .Where(e => e.CertificationId == certification.Id)
                    .ToList();
                _db.UserCertifications.RemoveRange(usersByCertification);

                foreach (string userId in Request.Form["user"]) {
                    User user = await _userManager.FindByIdAsync(userId);
                    UserCertifications userCertifications = new UserCertifications {
                        UserId = userId,
                        CertificationId = certification.Id,
                        User = user,
                        Certification = certification
                    };

                    await _db.UserCertifications.AddAsync(userCertifications);
                }

                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            
            return View(model);
        }

        /// <summary>
        /// Обработчик POST-запроса на удаление аттестации.
        /// </summary>
        /// <param name="id">Идентификатор аттестации.</param>
        /// <returns>Страницу с аттестациями.</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id) {
            Certification certification = await _db.Certifications.FindAsync(id);
            _db.Certifications.Remove(certification);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}