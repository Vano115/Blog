using Blog.Data.Models.UserModels;
using Blog.Data.Entityes;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// Конвертация данных модели в сущность пользователя
        /// </summary>
        /// <param name="user">Текущий пользователь</param>
        /// <param name="model">Модель со страницы</param>
        /// <returns></returns>
        public static ViewUserModel ConvertToModel(this ViewUserModel model, User user)
        {
            model.UserName = user.UserName;
            model.ProfileImage = user.ProfileImage;

            return model;
        }

        /// <summary>
        /// Конвертация данных модели в сущность пользователя
        /// </summary>
        /// <param name="user">Текущий пользователь</param>
        /// <param name="model">Модель со страницы</param>
        /// <returns></returns>
        public static UserEditViewModel ConvertToModel(this UserEditViewModel model, User user)
        {
            model.ProfileImage = user.ProfileImage;

            return model;
        }

        /// <summary>
        /// Обновление данных существующего пользователя из модели
        /// Нулевые значения модели не будут отражены на пользователе
        /// </summary>
        /// <param name="user">Текущий пользователь</param>
        /// <param name="model">Модель данных с сайта</param>
        /// <returns></returns>
        public static UserEditViewModel UpdateFromModel(this User user, UserEditViewModel model)
        {
            user.ProfileImage = string.IsNullOrEmpty(model.ProfileImage) ? user.ProfileImage : model.ProfileImage;

            return model;
        }
    }
}
