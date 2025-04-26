using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.BusinessLogic
{
    public class RepoLogic : IRepoLogic
    {
        RepoStorage repoStorage;

        public RepoLogic()
        {
            repoStorage = new RepoStorage();
        }
        public async Task<bool> Create(RepoBindingModel model)
        {
            if (!validate(model))
                return false;
            return await repoStorage.Create(model);
        }

        public bool Delete(long id)
        {
            return repoStorage.Delete(id);
        }

        public async Task<RepoViewModel?> Get(long id)
        {
            return await repoStorage.Get(id);
        }

        public async Task<List<RepoViewModel>> GetList(long id)
        {
            return await repoStorage.GetList(id);
        }

        public async Task<bool> Update(RepoBindingModel model)
        {
            if (validate(model))
                return false;
            return await repoStorage.Update(model);
        }

        private bool validate(RepoBindingModel model)
        {
            if (model.title.Length == 0)
                return false;
            return true;
        }
    }
}
