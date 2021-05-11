using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CertificationCenter.ViewModels
{
    public class QuestionViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Требуется заполнить поле Вопрос!"), Display(Name = "Вопрос")]
        public string QuestionString { get; set; }
        [Required(ErrorMessage = "Нужно заполнить ответ на вопрос!"), Display(Name = "Ответ")]
        public string AnswerString { get; set; }
    }
}