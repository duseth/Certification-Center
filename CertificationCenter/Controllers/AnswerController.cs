using CertificationCenter.Models;
using CertificationCenter.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificationCenter.Controllers
{
    public class AnswerController : Controller
    {

        private readonly ApplicationContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AnswerController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(EditUserViewModel model)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            IEnumerable<Answer> answers = _db.Answers.Where(a => a.UserId==user.Id);    //посмотреть, как вытаскивают юзер айди        
            return View(answers);
        }

        [HttpGet]
        public IActionResult IndexAdm()
        {
            IEnumerable<Certification> certifications = _db.Certifications;
            return View(certifications);
        }
        
    }
}
