using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;
using System.Text.RegularExpressions;

namespace PortfolioT.BusinessLogic
{
    public class UserLogic : IUserLogic
    {
        UserStorage userStorage;

        public UserLogic()
        {
            userStorage = new UserStorage();
        }
        public bool Create(UserBindingModel model)
        {
            if (validate(model))
                return false;
            return userStorage.Create(model);
        }

        public bool Delete(long id)
        {
            return userStorage.Delete(id);
        }

        public async Task<List<UserViewModel>> FindByLogin(string search)
        {
            if (search.Length == 0)
                return await userStorage.GetList();
            return await userStorage.FindByLogin(search);
        }

        public async Task<UserViewModel?> Get(long id)
        {
            return await userStorage.Get(id);
        }

        public async Task<List<UserViewModel>> GetList()
        {
            return await userStorage.GetList();
        }

        public async Task<bool> Update(UserBindingModel model)
        {
            validate(model, true);
            return await userStorage.Update(model);
        }
        public bool validate(UserBindingModel model, bool update = false)
        {
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            if (model.login.Length == 0)
                return false;
            if (model.password.Length == 0)
                return false;
            if (!Regex.IsMatch(model.email, pattern, RegexOptions.IgnoreCase))
                return false;
            if (update &&
                (userStorage.checkByEmail(model.email) || userStorage.checkByLogin(model.login)))
                return false;

            return true;
        }
    }
}
