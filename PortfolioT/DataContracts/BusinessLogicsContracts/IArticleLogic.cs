using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.BusinessLogicsContracts
{
    public interface IArticleLogic
    {
        Task<List<ArticleViewModel>> GetList(long id);
        Task<ArticleViewModel> Get(long id);
        Task<bool> Create(ArticleBindingModel model);
        Task<bool> Update(ArticleBindingModel model);
        bool Delete(long id);
    }
}
