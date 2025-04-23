using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.BusinessLogicsContracts
{
    public interface IArticleStorage
    {
        List<ArticleViewModel> GetList();
        ArticleViewModel Get(long id);
        bool Create(ArticleBindingModel model);
        bool Update(ArticleBindingModel model);
        bool Delete(ArticleBindingModel model);
    }
}
