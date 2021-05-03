using System.ComponentModel.DataAnnotations;

namespace CertificationCenter.ViewModels {
    public class LoginViewModel {
        [Required(ErrorMessage = "Требуется заполнить поле Email!"), DataType(DataType.EmailAddress), Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Требуется заполнить поле Пароль!"), DataType(DataType.Password), Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}