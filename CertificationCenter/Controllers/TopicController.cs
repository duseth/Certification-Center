using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CertificationCenter.Models;
using CertificationCenter.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CertificationCenter.Controllers
{
    public class TopicController : Controller
    {
        private readonly ApplicationContext _db;

        public TopicController(ApplicationContext db)
        {
            _db = db;
        }

        // GET: TopicController
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Index()
        {
            List<Topic> topics = _db.Topics.ToList();
            return View(topics);
        }



        // GET: TopicController/Create
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TopicController/Create
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(CreateTopicViewModel model)
        {

            if (model.Questions.Count != 0)
            {
                Topic topic = new Topic
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    
                };

                await _db.Topics.AddAsync(topic);
                foreach (var item in model.Questions)
                {
                    Question question = new Question()
                    {
                        Id=Guid.NewGuid().ToString(),
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

        // GET: TopicController/Edit/5
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
        {
            Topic topic = await _db.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            var questions = _db.Questions.Where(x => x.TopicId == topic.Id).ToList();
            List<QuestionsViewModel> questionsViewModel=new List<QuestionsViewModel>();
            foreach (var item in questions)
            {
                questionsViewModel.Add(new QuestionsViewModel()
                {
                    QuestionString = item.QuestionString,
                    AnswerString = item.AnswerString
                });
            }

            EditTopicViewModel viewModel = new EditTopicViewModel()
            {
                Name = topic.Name,
                Questions = questionsViewModel
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(EditTopicViewModel model)
        {

            if (model.Questions.Count != 0)
            {
                Topic topic = await _db.Topics.FindAsync(model.Id);
                if (topic == null)
                {
                    return NotFound();
                }

                topic.Name = model.Name;

                var listQuestions = _db.Questions.Where(x => x.TopicId == topic.Id).ToList();
                _db.Questions.RemoveRange(listQuestions);
                foreach (var item in model.Questions)
                {
                    Question question = new Question()
                    {
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

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            Topic topic = await _db.Topics.FindAsync(id);
            _db.Topics.Remove(topic);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
    
}
