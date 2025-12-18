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
        [StringLength(24, ErrorMessage = "{0} от {2} и до {1} символов, а так же 1 спецсимвол(! # - и т.д.).", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public required string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public required string PasswordConfirm { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public required string Email { get; set; }


        public RegisterViewModel() { }
    }
}
