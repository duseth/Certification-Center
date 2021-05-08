using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CertificationCenter.Models;
using CertificationCenter.ViewModels;

namespace CertificationCenter.Controllers {
    /// <summary>
    /// Контроллер для управления домашней страницей.
    /// </summary>
    public class HomeController : Controller {
        /// <summary>
        /// Обработчик GET-запроса при обращении на домашнюю страницу.
        /// </summary>
        /// <returns>Домашнюю страницу.</returns>
        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        /// <summary>
        /// Обработчик GET-запроса при ошибке.
        /// </summary>
        /// <returns>Страницу с ошибкой.</returns>
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}