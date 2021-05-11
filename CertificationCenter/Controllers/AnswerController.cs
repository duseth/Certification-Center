using CertificationCenter.Models;
using CertificationCenter.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> UserCert(EditUserViewModel model)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            IEnumerable<Certification> certifications =null;
            IEnumerable<Answer> answers = _db.Answers.Where(a => a.UserId == user.Id);
            foreach (Answer answer in answers)
            { 
                certifications = _db.Certifications.Where(c => c.Id == answer.CertificationId);
            }
           
            return View(certifications);
        }

        [HttpGet]
        public async Task<IActionResult> Index(EditUserViewModel model, string id)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            IEnumerable<Answer> answers = _db.Answers.Include(a => a.UserId==user.Id).Include(a => a.CertificationId==id);        
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
