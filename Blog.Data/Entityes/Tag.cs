using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Entityes
{
    public class Tag
    {
        public int Id { get; set; } = 0;

        public required string Name { get; set; }

        public List<Article> Articles { get; } = [];
    }
}
