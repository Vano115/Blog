namespace Blog.Models.UserModels
{
    public class AutorizeViewModel
    {
        public required string UserName { get; set; }

        public required string Password { get; set; }

        public required bool RememberMe { get; set; } = false;
    }
}
