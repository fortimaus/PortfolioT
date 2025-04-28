using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.StorageContracts
{
    public interface IRepoStorage
    {
        Task<List<RepoViewModel>> GetList(long id);
        Task<RepoViewModel> Get(long id);
        Task<bool> Create(RepoBindingModel model);
        Task<bool> Update(RepoBindingModel model);
        bool Delete(long id);
        void DeleteAll(long id);
        void DeleteByService(long userId, long serviceId);
    }
}
