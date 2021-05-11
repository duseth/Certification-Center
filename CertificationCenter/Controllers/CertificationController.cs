using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CertificationCenter.Models;
using CertificationCenter.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CertificationCenter.Controllers {
    [Authorize]
    public class CertificationController : Controller {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _db;

        public CertificationController(UserManager<User> userManager, ApplicationContext db) {
            _userManager = userManager;
            _db = db;
        }

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

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Create() {
            return View();
        }
       

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateCertificationViewModel model) {
            if (ModelState.IsValid) {
                if (model.DatetimeEnd != null) {
                    Certification certification = new Certification {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name,
                        Description = model.Description,
                        DatetimeStart = DateTime.Today,
                        DatetimeEnd = (DateTime) model.DatetimeEnd,
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
                        foreach (var item in model.Questions)
                        {
                            Question question = new Question()
                            {
                                Id = Guid.NewGuid().ToString(),
                                QuestionString = item.QuestionString,
                                AnswerString = item.AnswerString,
                                Certification = certification,
                                CertificationId = certification.Id
                            };
                            await _db.Questions.AddAsync(question);
                        }
                    }



                    await _db.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id) {
            Certification certification = await _db.Certifications.FindAsync(id);

            var users = _db.UserCertifications
                .Where(e => e.CertificationId == certification.Id)
                .Select(e => e.UserId)
                .ToArray();
            var questions = _db.Questions.Where(x => x.CertificationId == certification.Id).ToList();
            List<QuestionViewModel> questionsViewModel = new List<QuestionViewModel>();
            foreach (var item in questions)
            {
                questionsViewModel.Add(new QuestionViewModel()
                {
                    QuestionString = item.QuestionString,
                    AnswerString = item.AnswerString
                });
            }
            EditCertificationViewModel editCertificationViewModel = new EditCertificationViewModel {
                Id = id,
                Name = certification.Name,
                Description = certification.Description,
                DatetimeEnd = certification.DatetimeEnd,
                Users = users,
                Questions = questionsViewModel
            };

            return View(editCertificationViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Pass(string id)
        {
            Certification certification = await _db.Certifications.FindAsync(id);
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            var questions = _db.Questions.Where(x => x.CertificationId == certification.Id)
                .Select(x => x.QuestionString).ToList();
            PassCertificationViewModel passCertification=new PassCertificationViewModel()
            {
                IdUser = user.Id,
                IdCertification = certification.Id,
                Name = certification.Name,
                Questions = questions,
            };
            return View(passCertification);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> Pass(PassCertificationViewModel  model)
        {
            if (model.Answer != null)
            {
                Certification certification = await _db.Certifications.FindAsync(model.IdCertification);
                var questions =  _db.Questions.Where(x => x.CertificationId == certification.Id).ToList();
                foreach (var quest in questions)
                {

                    foreach (var item in model.Answer)
                    {

                        Answer answer = new Answer()
                        {
                            UserId = model.IdUser,
                            AnswerString = item.Answer,
                            Certification = certification,
                            CertificationId = certification.Id,
                            Question = quest,
                            User = await _userManager.FindByIdAsync(model.IdUser),
                            IsCorrect = item.Answer.ToLower()==quest.AnswerString.ToLower()?true:false
                        };
                        await _db.Answers.AddAsync(answer);
                    }
                }

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

       
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(EditCertificationViewModel model) {
            if (model.DatetimeEnd != null) {
                Certification certification = await _db.Certifications.FindAsync(model.Id);

                if (certification != null) {
                    certification.Name = model.Name;
                    certification.Description = model.Description;
                    certification.DatetimeEnd = (DateTime) model.DatetimeEnd;

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
            }

            return View();
        }

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