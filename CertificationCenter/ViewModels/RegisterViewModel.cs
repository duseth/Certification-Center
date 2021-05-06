using System.ComponentModel.DataAnnotations;

namespace CertificationCenter.ViewModels {
    public class RegisterViewModel {
        [Required(ErrorMessage = "Требуется заполнить поле Имя!"), DataType(DataType.Text), Display(Name = "Имя")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Требуется заполнить поле Email!"), DataType(DataType.EmailAddress), Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Требуется заполнить поле Пароль!"), DataType(DataType.Password), Display(Name = "Пароль")]
        public string Password { get; set; }
 
        [Required(ErrorMessage = "Требуется заполнить поле Подвердить пароль!"),
         Compare("Password", ErrorMessage = "Пароли не совпадают"),
         DataType(DataType.Password), Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}