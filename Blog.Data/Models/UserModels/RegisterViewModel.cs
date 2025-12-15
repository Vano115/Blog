using Blog.Data.Entityes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Blog.Data.Models.UserModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public required string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public required string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public required string PasswordConfirm { get; set; }

        [Required]
        [Display(Name = "Email")]
        public required string Email { get; set; }


        public RegisterViewModel() { }
    }
}
