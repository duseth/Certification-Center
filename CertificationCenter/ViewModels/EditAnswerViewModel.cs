using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CertificationCenter.Models;

namespace CertificationCenter.ViewModels
{
    public class EditAnswerViewModel
    {
        public string Id { get; set; }
        public List<Answer> Answers { get; set; }
        public List<string> ChangeStringList { get; set; }
    }
}
