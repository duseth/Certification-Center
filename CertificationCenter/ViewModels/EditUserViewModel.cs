using System.ComponentModel.DataAnnotations;

namespace CertificationCenter.ViewModels {
    public class EditUserViewModel {
        public string Id { get; set; }
        [Required(ErrorMessage = "Имя пользователя не заполнено"), Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email обязательный для заполнения"), Display(Name = "Email"),
         DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Пароль"), DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Роль")]
        public string Role { get; set; }
    }
}