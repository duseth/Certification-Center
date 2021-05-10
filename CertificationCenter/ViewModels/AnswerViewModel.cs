using CertificationCenter.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificationCenter.ViewModels
{
    public class AnswerViewModel
    {
        public Answer answer { get; set; }
        public IEnumerable<SelectListItem> AnswerList { get; set; }
    }
}
