using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Entityes
{
    public class Comment
    {
        public int Id { get; set; }

        public required string Text { get; set; }

        public required DateTime PublicDate { get; set; }

        public int ArticleId { get; set; } = 0;
        public required Article Article { get; set; }

        public string UserId {  get; set; } = string.Empty;
        public required User User { get; set; }

        public int Likes { get; set; }

        public Comment()
        {
            PublicDate = DateTime.UtcNow;
        }
    }
}
