using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.StorageContracts
{
    public interface IUserServiceStorage
    {
        List<UserServiceViewModel> GetUserList(long id);
        bool Create(UserServiceBindingModel model);
        bool Delete(UserServiceBindingModel model);

        bool Update(UserServiceBindingModel model);
    }
}
