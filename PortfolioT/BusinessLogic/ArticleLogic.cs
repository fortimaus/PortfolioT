using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.BusinessLogic
{
    public class ArticleLogic : IArticleLogic
    {
        ArticleStorage articleStorage;

        public ArticleLogic()
        {
            articleStorage = new ArticleStorage();
        }
        public async Task<bool> Create(ArticleBindingModel model)
        {
            if (!validate(model))
                return false;
            return await articleStorage.Create(model);
        }

        public bool Delete(long id)
        {
            return articleStorage.Delete(id);
        }

        public async Task<ArticleViewModel?> Get(long id)
        {
            return await articleStorage.Get(id);
        }

        public async Task<List<ArticleViewModel>> GetList(long id)
        {
            return await articleStorage.GetList(id);
        }

        public async Task<bool> Update(ArticleBindingModel model)
        {
            if (validate(model))
                return false;
            return await articleStorage.Update(model);
        }

        private bool validate(ArticleBindingModel model)
        {
            if (model.title.Length == 0)
                return false;
            return true;
        }
    }
}
