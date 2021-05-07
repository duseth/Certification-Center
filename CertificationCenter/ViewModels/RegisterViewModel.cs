using System.ComponentModel.DataAnnotations;

namespace CertificationCenter.ViewModels {
    public class RegisterViewModel {
        [Required(ErrorMessage = "Имя пользователя не заполнено"), Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Email обязательный для заполнения"), DataType(DataType.EmailAddress), Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Поле пароля не должно быть пустым"), DataType(DataType.Password), Display(Name = "Пароль")]
        public string Password { get; set; }
 
        [Required(ErrorMessage = "Подтверждение пароля обязательно"),
         Compare("Password", ErrorMessage = "Пароли не совпадают"),
         DataType(DataType.Password), Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}