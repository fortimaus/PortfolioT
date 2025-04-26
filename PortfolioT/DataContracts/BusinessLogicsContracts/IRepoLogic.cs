using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.BusinessLogicsContracts
{
    public interface IRepoLogic
    {
        Task<List<RepoViewModel>> GetList(long id);
        Task<RepoViewModel> Get(long id);
        Task<bool> Create(RepoBindingModel model);
        Task<bool> Update(RepoBindingModel model);
        bool Delete(long id);
    }
}
