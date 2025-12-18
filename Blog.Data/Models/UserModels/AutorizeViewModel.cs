using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Models.UserModels
{
    public class AutorizeViewModel
    {
        [Required]
        [Display(Name = "Логин")]
        public required string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public required string Password { get; set; }

        [Required]
        [Display(Name = "Запомнить меня")]
        public required bool RememberMe { get; set; } = false;
    }
}
