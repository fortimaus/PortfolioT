using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.StorageContracts
{
    public interface IArticleStorage
    {
        List<ArticleViewModel> GetList();
        ArticleViewModel Get(long id);
        bool Create(ArticleBindingModel model);
        bool Update(ArticleBindingModel model);
        bool Delete(long id);
    }
}
