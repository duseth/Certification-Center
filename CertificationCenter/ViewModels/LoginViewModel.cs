using System.ComponentModel.DataAnnotations;

namespace CertificationCenter.ViewModels {
    public class LoginViewModel {
        [Required(ErrorMessage = "Вы не ввели Email"), DataType(DataType.EmailAddress), Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Вы не ввели пароль"), DataType(DataType.Password), Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}