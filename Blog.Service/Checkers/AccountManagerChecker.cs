using Blog.Data.Entityes;
using Blog.Data.UoW;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Blog.Service.Exceptions.AccountManager;
using Blog.Data.Models.UserModels;

namespace Blog.Service.Checkers
{
    public class AccountManagerChecker
    {

        private readonly UserManager<User> _userManager;

        private readonly IEnumerable<string> _blackList =
            [
            "admin",
            "moderator",
            "support",
            "helpdesk"
            ];

        private readonly IEnumerable<char> _whiteList = "abcdefghijklmnopqrstuvwxyz!@#*-_=+[]<>\\0123456789".ToCharArray();

        public AccountManagerChecker (UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> CheckModel(RegisterViewModel model)
        {
            var login = await CheckLogin(model.UserName);
            var password = await CheckPassword(model.Password);
            var Email = await CheckEmail(model.Email);

            return login && password & Email;
        }

        /// <summary>
        /// Проверка полученного UserName
        /// </summary>
        /// <param name="str">UserName из модели с сайта</param>
        /// <returns>true если введено корректное значение, false если нет</returns>
         private async Task<bool> CheckLogin (string str)
        {
            // Первый символ - буква или цифра
            bool checkStartName = char.IsAsciiLetterOrDigit(str[0]);

            // Длинна от 6 до 20
            bool checkNameLength = str.Length > 5 && str.Length < 21;

            // Только разрешённые символы
            bool checkSimbols = str.ToLower().Any(c => _whiteList.Contains(c));

            // Недопустимые имена
            bool checkBlackList = !_blackList.Contains(str);

            // Не должно быть совпадений с существующими
            bool checkNameExists = await _userManager.FindByNameAsync(str) == null ? true : 
                throw new LoginBusyException($"Логин {str} уже занят");

            if (checkStartName && checkNameLength && checkSimbols && checkBlackList && checkNameExists)
            {
                return true;
            }

            throw new WrongValueException("Некорректный логин");
        }

        /// <summary>
        /// Проверка полученного Password
        /// </summary>
        /// <param name="str">Password из модели с сайта</param>
        /// <returns>true если введено корректное значение, false если нет</returns>
        private async Task<bool> CheckPassword(string str)
        {
            // Первый символ - буква или цифра
            bool checkStartName = char.IsAsciiLetterOrDigit(str[0]);

            // Длинна от 8 до 24
            bool checkNameLength = str.Length > 7 && str.Length < 25;

            // Только разрешённые символы
            bool checkSimbols = str.ToLower().Any(c => _whiteList.Contains(c));

            if (checkStartName && checkNameLength && checkSimbols)
            {
                return true;
            }

            throw new WrongValueException("Некорректный пароль");
        }

        /// <summary>
        /// Проверка полученного Email
        /// </summary>
        /// <param name="str">Password из модели с сайта</param>
        /// <returns>true если введено корректное значение, false если нет</returns>
        private async Task<bool> CheckEmail(string str)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(str, pattern, RegexOptions.IgnoreCase);

            if (!isMatch.Success) { throw new WrongValueException("Некорректно введен Email"); }
            return isMatch.Success;
        }

    }
}
