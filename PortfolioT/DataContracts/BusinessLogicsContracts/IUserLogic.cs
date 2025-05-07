using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.BusinessLogicsContracts
{
    public interface IUserLogic
    {
        Task<List<UserViewModel>> GetList();
        Task<List<UserViewModel>> FindByLogin(string search);
        Task<UserViewModel> Get(long id);
        Task<long> Create(UserBindingModel model);
        Task<bool> Update(UserBindingModel model);
        bool Delete(long id);
    }
}
