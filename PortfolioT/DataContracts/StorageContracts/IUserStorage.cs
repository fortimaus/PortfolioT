using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.StorageContracts
{
    public interface IUserStorage
    {
        Task<List<UserViewModel>> GetList();
        Task<List<UserViewModel>> FindByLogin(string search);
        Task<UserViewModel> Get(long id);
        Task<long> Create(UserBindingModel model);
        Task<bool> Update(UserBindingModel model);
        bool Delete(long id);
    }
}
