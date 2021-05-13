using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CertificationCenter.ViewModels {
    public class EditTopicViewModel {
        public string Id { get; set; }

        [Required(ErrorMessage = "Название темы не заполнено"), Display(Name = "Название темы")]
        public string Name { get; set; }

        public List<QuestionsViewModel> Questions { get; set; }
    }
}