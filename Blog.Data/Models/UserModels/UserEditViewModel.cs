using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.Models.UserModels
{
    public class UserEditViewModel
    {
        public string ProfileImage { get; set; } = string.Empty;

        public List<string> Images { get; set; } = new List<string>();
    }
}
