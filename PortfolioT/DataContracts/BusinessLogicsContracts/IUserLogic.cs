using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.BusinessLogicsContracts
{
    public interface IUserLogic
    {
        List<UserViewModel> GetList();
        List<UserViewModel> FindByLogin(string search);
        UserViewModel Get(long id);
        bool Create(UserBindingModel model);
        bool Update(UserBindingModel model);
        bool Delete(UserBindingModel model);
    }
}
