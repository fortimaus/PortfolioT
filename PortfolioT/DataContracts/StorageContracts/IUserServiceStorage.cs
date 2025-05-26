using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.StorageContracts
{
    public interface IUserServiceStorage
    {
        List<UserServiceViewModel> GetUserList(long id);
        bool Create(UserServiceBindingModel model);
        bool Delete(long userId, long serviceId);

        bool Update(UserServiceBindingModel model);
    }
}
