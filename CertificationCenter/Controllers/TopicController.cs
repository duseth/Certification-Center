using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CertificationCenter.Models;
using CertificationCenter.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CertificationCenter.Controllers {
    /// <summary>
    /// Контроллер для работы с темами аттестаций.
    /// </summary>
    public class TopicController : Controller {
        /// <summary>
        /// Свойство предоставляющее контекст для работы с базой данных.
        /// </summary>
        private readonly ApplicationContext _db;

        public TopicController(ApplicationContext db) {
            _db = db;
        }

        /// <summary>
        /// Обработчик GET-запроса для получения всех тем.
        /// </summary>
        /// <returns>Список всех тем.</returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Index() {
            List<Topic> topics = _db.Topics.ToList();
            return View(topics);
        }
        
        /// <summary>
        /// Обработчик GET-запрос для получения формы создания темы.
        /// </summary>
        /// <returns>Форму создания темы.</returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create() {
            return View();
        }
        
        /// <summary>
        /// Обработчик POST-запрос на создание темы.
        /// </summary>
        /// <param name="model">Модель представления создания темы.</param>
        /// <returns>Перенаправление на страницу списка тем при успешном создании темы,
        /// иначе страницу с ошибкой.</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateTopicViewModel model) {
            if (ModelState.IsValid && model.Questions.Count != 0) {
                Topic topic = new Topic {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                };

                await _db.Topics.AddAsync(topic);
                foreach (var item in model.Questions) {
                    Question question = new Question() {
                        Id = Guid.NewGuid().ToString(),
                        QuestionString = item.QuestionString,
                        AnswerString = item.AnswerString,
                        Topic = topic,
                        TopicId = topic.Id
                    };
                    await _db.Questions.AddAsync(question);
                }

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }
        
        /// <summary>
        /// Обработчик GET-запроса для изменения темы.
        /// </summary>
        /// <param name="id">Идентификатор темы.</param>
        /// <returns>Страницу изменения темы.</returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id) {
            Topic topic = await _db.Topics.FindAsync(id);
            if (topic == null) {
                return NotFound();
            }

            var questions = _db.Questions.Where(x => x.TopicId == topic.Id).ToList();
            List<QuestionsViewModel> questionsViewModel = new List<QuestionsViewModel>();
            foreach (var item in questions) {
                questionsViewModel.Add(new QuestionsViewModel() {
                    QuestionString = item.QuestionString,
                    AnswerString = item.AnswerString
                });
            }

            EditTopicViewModel viewModel = new EditTopicViewModel() {
                Name = topic.Name,
                Questions = questionsViewModel
            };

            return View(viewModel);
        }

        /// <summary>
        /// Обработчик POST-запроса на изменение темы.
        /// </summary>
        /// <param name="model">Модель представления изменения темы.</param>
        /// <returns>Перенаправление на страницу с темами, если теста не существует - страницу с ошибкой.</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(EditTopicViewModel model) {
            if (ModelState.IsValid && model.Questions.Count != 0) {
                Topic topic = await _db.Topics.FindAsync(model.Id);
                if (topic == null) {
                    return NotFound();
                }

                topic.Name = model.Name;

                var listQuestions = _db.Questions.Where(x => x.TopicId == topic.Id).ToList();
                _db.Questions.RemoveRange(listQuestions);
                foreach (var item in model.Questions) {
                    Question question = new Question() {
                        Id = Guid.NewGuid().ToString(),
                        QuestionString = item.QuestionString,
                        AnswerString = item.AnswerString,
                        Topic = topic,
                        TopicId = topic.Id
                    };
                    await _db.Questions.AddAsync(question);
                }

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        /// <summary>
        /// Обработчик POST-запроса на удаление темы.
        /// </summary>
        /// <param name="id">Идентификатор темы.</param>
        /// <returns>Перенаправление на страницу с темами.</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id) {
            Topic topic = await _db.Topics.FindAsync(id);
            _db.Topics.Remove(topic);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}