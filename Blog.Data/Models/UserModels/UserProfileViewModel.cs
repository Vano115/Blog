using Blog.Data.Entityes;

namespace Blog.Data.Models.UserModels
{
    public class UserProfileViewModel
    {
        public required string UserName { get; set; }

        public string ProfileImage { get; set; } = string.Empty;

        public List<Article> Articles { get; set; } = [];
    }
}
