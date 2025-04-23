using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.BusinessLogicsContracts
{
    public interface IRepoStorage
    {
        List<RepoViewModel> GetList();
        RepoViewModel Get(long id);
        bool Create(RepoBindingModel model);
        bool Update(RepoBindingModel model);
        bool Delete(RepoBindingModel model);
    }
}
