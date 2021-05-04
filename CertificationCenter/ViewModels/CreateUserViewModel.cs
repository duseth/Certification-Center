using System.ComponentModel.DataAnnotations;

namespace CertificationCenter.ViewModels {
    public class CreateUserViewModel {
        [Required(ErrorMessage = "Имя пользователя не заполнено"), Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email обязательный для заполнения"), Display(Name = "Email"),
        DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Поле пароля не должно быть пустым"), Display(Name = "Пароль"),
         DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Роль должна быть указана"), Display(Name = "Роль")]
        public string Role { get; set; }
    }
}