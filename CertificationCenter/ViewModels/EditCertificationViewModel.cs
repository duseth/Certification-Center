using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CertificationCenter.DataAnnotations;

namespace CertificationCenter.ViewModels {
    public class EditCertificationViewModel {
        public string Id { get; set; }

        [Required(ErrorMessage = "Название не заполнено"), Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Требуется указать дату окончания"),
         DataType(DataType.Date),
         Display(Name = "Дата окончания"),
         CurrentDate(ErrorMessage = "Дата окончания должна быть больше текущего дня")]
        public DateTime? DatetimeEnd { get; set; }
        public string[] Users { get; set; }

        public List<QuestionViewModel> Questions { get; set; }
    }
}