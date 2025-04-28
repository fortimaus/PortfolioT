using PortfolioT.BusinessLogic.Exceptions;
using PortfolioT.DataBase.Models;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.Services.GitService;

namespace PortfolioT.BusinessLogic.Logics
{
    public class UserServiceLogic : IUserServiceLogic
    {
        GitService service;
        UserServiceStorage serviceStorage; 
        public UserServiceLogic()
        {
            serviceStorage = new UserServiceStorage();
            service = new GitService();
        }
        public bool Create(UserServiceBindingModel model)
        {
            try
            {
                validate(model);
                return serviceStorage.Create(model);
            }
            catch
            {
                throw;
            }

        }

        public bool Delete(UserServiceBindingModel model)
        {
            try
            {
                return serviceStorage.Delete(model);

            }
            catch
            {
                throw;
            }
        }

        public bool Update(UserServiceBindingModel model)
        {
            try
            {
                validate(model);
                return serviceStorage.Update(model);
            }
            catch
            {
                throw;
            }

        }

        public List<UserServiceViewModel> GetUserList(long id)
        {
            try
            {
                return serviceStorage.GetUserList(id);

            }
            catch
            {
                throw;
            }
        }

        private async void validate(UserServiceBindingModel model)
        {
            if (model.data.Length == 0)
                throw new InvalidException("Данные не должны быть пустыми");
        }
    }
}
