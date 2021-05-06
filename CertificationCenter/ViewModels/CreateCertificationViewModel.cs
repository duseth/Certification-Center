using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CertificationCenter.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CertificationCenter.ViewModels {
    public class CreateCertificationViewModel {
        [Required(ErrorMessage = "Требуется заполнить поле Имя!"), Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Требуется заполнить поле Даты окончания!"),
         DataType(DataType.Date),
         Display(Name = "Дата окончания"),
         CurrentDate(ErrorMessage = "Дата окончания должна быть больше текущего дня")]
        public DateTime? DatetimeEnd { get; set; }

        public List<CreateQuestionsViewModel> Questions { get; set; }
    }
}