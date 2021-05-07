using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CertificationCenter.ViewModels
{
    public class CreateQuestionsViewModel
    {
        [Required(ErrorMessage = "Требуется заполнить поле Имя!"), Display(Name = "Вопрос")]
        public string QuestionString { get; set; }
        [Required(ErrorMessage = "Нужно заполнить ответ на вопрос!"), Display(Name = "Ответ")]
        public string AnswerString { get; set; }
        public List<CreateQuestionsViewModel> AddMoreList { get; set; }
    }
}