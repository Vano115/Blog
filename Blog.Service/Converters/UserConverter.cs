using Blog.Data.Models.UserModels;
using Blog.Data.Entityes;

namespace Blog.Models.UserModels
{
    public static class UserConverter
    {
        /// <summary>
        /// Конвертация данных модели в сущность пользователя
        /// </summary>
        /// <param name="user">Текущий пользователь</param>
        /// <param name="model">Модель со страницы</param>
        /// <returns></returns>
        public static User ConvertToUser(this User user, RegisterViewModel model)
        {
            user.Email = model.Email;
            user.UserName = model.UserName;

            return user;
        }
    }
}
