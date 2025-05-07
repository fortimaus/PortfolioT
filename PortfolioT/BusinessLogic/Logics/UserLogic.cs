using PortfolioT.BusinessLogic.Exceptions;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;
using PortfolioT.MailWorker;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PortfolioT.BusinessLogic.Logics
{
    public class UserLogic : IUserLogic
    {
        UserStorage userStorage;
        MailKitWorker kitWorker;
        public UserLogic()
        {
            userStorage = new UserStorage();
            kitWorker = MailKitWorker.getInstance();
        }
        private string generateCode()
        {
            Random random = new Random();
            return $"{random.Next(0,10)}{random.Next(0, 10)}{random.Next(0, 10)}{random.Next(0, 10)}{random.Next(0, 10)}{random.Next(0, 10)}";
        }
        private void sendEmail(string email,string code)
        {
            kitWorker.MailSendAsync(new MailSendInfoBindingModel()
            {
                MailAddress = email,
                Subject = "Авторизация в системе PortfolioT",
                Text = "<!DOCTYPE html>\r\n<html lang=\"ru\">\r\n<head>\r\n    " +
                    "<meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    " +
                    "<style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            line-height: 1.6;\r\n            color: #333333;\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            padding: 20px;\r\n        }\r\n        .header {\r\n            text-align: center;\r\n            margin-bottom: 30px;\r\n        }\r\n        .code-container {\r\n            background-color: #f5f5f5;\r\n            border-radius: 5px;\r\n            padding: 15px;\r\n            text-align: center;\r\n            margin: 20px 0;\r\n            font-size: 24px;\r\n            font-weight: bold;\r\n            color: #2c3e50;\r\n        }\r\n        .footer {\r\n            margin-top: 30px;\r\n            font-size: 12px;\r\n            color: #777777;\r\n            text-align: center;\r\n        }\r\n    </style>\r\n</head>\r\n" +
                    "<body>\r\n    " +
                    "<div class=\"header\">\r\n        " +
                    "<h1>Ваш код авторизации</h1>\r\n    </div>\r\n    " +
                    "<p>Здравствуйте,</p>\r\n    \r\n    " +
                    "<p>Мы получили запрос на авторизацию вашей учетной записи. Используйте следующий код для подтверждения:</p>\r\n    \r\n   " +
                    $" <div class=\"code-container\">\r\n        <!-- Код будет вставлен здесь -->\r\n        {code}\r\n    </div>\r\n    \r\n    " +
                    "<p>Если вы не запрашивали этот код, пожалуйста, проигнорируйте это письмо или свяжитесь с нашей службой поддержки.</p>\r\n    \r\n    \r\n    <div class=\"footer\">\r\n        <p>Если у вас есть вопросы, напишите нам на <a href=\"mailto:support@example.com\">support@example.com</a></p>\r\n    </div>\r\n</body>\r\n</html>"
            });
        }
        public async Task<long> Create(UserBindingModel model)
        {
            try
            {
                validate(model);
                string code = generateCode();
                model.code = code;
                sendEmail(model.email, code);
                return await userStorage.Create(model);
            }
            catch
            {
                throw;
            }

        }
        public bool CheckCode(long id, string code)
        {
            try
            {
                if (userStorage.CheckByIdAndCode(id, code))
                    return true;
                return false;
            }catch(Exception ex)
            {
                throw;
            }
        }
        public bool Delete(long id)
        {
            try
            {
                return userStorage.Delete(id);

            }
            catch
            {
                throw;
            }
        }

        public async Task<List<UserViewModel>> FindByLogin(string search)
        {
            try
            {
                if (search.Length == 0)
                    return await userStorage.GetList();
                return await userStorage.FindByLogin(search);
            }
            catch
            {
                throw;
            }

        }

        public async Task<UserViewModel> FindByLoginAndPassword(string login, string password)
        {
            try
            {
                return await userStorage.GetByLoginAndPassword(login,password);
            }
            catch
            {
                throw;
            }
            
        }

        public bool UpdateRole(long id, UserRole role)
        {
            try
            {
                return userStorage.UpdateRole(id, role);

            }
            catch
            {
                throw;
            }
        }

        public bool UpdateStatus(long id, UserStatus status)
        {
            try
            {
                return userStorage.UpdateStatus(id, status);

            }
            catch
            {
                throw;
            }
        }

        public async Task<UserViewModel?> Get(long id)
        {
            try
            {
                return await userStorage.Get(id);

            }
            catch
            {
                throw;
            }
        }

        public async Task<List<UserViewModel>> GetList()
        {
            try
            {
                return await userStorage.GetList();

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(UserBindingModel model)
        {
            try
            {
                validate(model, true);
                return await userStorage.Update(model);
            }
            catch
            {
                throw;
            }
   
        }
        public async Task<long> UpdateCodeForEmail(long id, string email)
        {
            try
            {
                string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
                if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
                    throw new InvalidException("Неверный формат почты");

                string code = generateCode();
                sendEmail(email, code);
                return await userStorage.UpdateCode(id, code);
            }
            catch
            {
                throw;
            }

        }
        public bool UpdateEmail(long id, string newEmail)
        {
            try
            {
                userStorage.UpdateEmail(id, newEmail);
                return true;
            }
            catch
            {
                throw;
            }
        }
        public void validate(UserBindingModel model, bool update = false)
        {
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            if (model.login.Length == 0)
                throw new InvalidException("Логин не должен быть пустым");
            if (model.password.Length == 0)
                throw new InvalidException("Пароль не должен быть пустым");
            if (!Regex.IsMatch(model.email, pattern, RegexOptions.IgnoreCase))
                throw new InvalidException("Неверный формат почты");
            if(!userStorage.checkByLogin(model.login))
                throw new InvalidException("Логин занят");
            if (!userStorage.checkByEmail(model.email))
                throw new InvalidException("Почта занята");
                
        }
    }
}
