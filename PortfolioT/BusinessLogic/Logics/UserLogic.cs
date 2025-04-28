using PortfolioT.BusinessLogic.Exceptions;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;
using System.Text.RegularExpressions;

namespace PortfolioT.BusinessLogic.Logics
{
    public class UserLogic : IUserLogic
    {
        UserStorage userStorage;

        public UserLogic()
        {
            userStorage = new UserStorage();
        }
        public async Task<bool> Create(UserBindingModel model)
        {
            try
            {
                validate(model);
                return await userStorage.Create(model);
            }
            catch
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
            if (!update)
            {
                userStorage.checkByEmail(model.email);
                userStorage.checkByLogin(model.login);
            }
                
        }
    }
}
