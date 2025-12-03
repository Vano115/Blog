using Blog.Data.Entityes;

namespace Blog.Models.BlogModels
{
    public class UserPageViewModel
    {
        public required string Nickname { get; set; }

        public List<Article> Articles { get; set; } = new List<Article>();

        public bool IsOnline { get; set; }
    }
}
