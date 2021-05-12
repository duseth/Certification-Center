using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CertificationCenter.Models;
using CertificationCenter.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
            public IActionResult UserCert(string id)
            {
                var result = _db.Results.Where(x => x.UserId == id).Include(x=>x.Certification).ToList();
                return View(result);
            }

            [HttpGet]
            public async Task<IActionResult> Index(string id)
            {
                var anwers = _db.Answers.Where(x => x.ResultId == id).Include(x=>x.Question).Include(x=>x.Result).ThenInclude(x=>x.Certification).ToList();
                return View(anwers);
            }

            [HttpGet]
            public IActionResult IndexAdm()
            {
                IEnumerable<Certification> certifications = _db.Certifications;
                return View(certifications);
            }

        }
    }

