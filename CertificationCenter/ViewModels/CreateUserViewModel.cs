using System.ComponentModel.DataAnnotations;

namespace CertificationCenter.ViewModels {
    public class CreateUserViewModel {
        [Required(ErrorMessage = "Требуется заполнить поле Имя!"), Display(Name = "Имя")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Требуется заполнить поле Email!"), Display(Name = "Email"),
        DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Требуется заполнить поле Пароль!"), Display(Name = "Пароль"),
         DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}