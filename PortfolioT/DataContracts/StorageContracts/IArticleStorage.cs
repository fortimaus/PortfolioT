using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.StorageContracts
{
    public interface IArticleStorage
    {
        Task<List<ArticleViewModel>> GetList(long id);
        Task<ArticleViewModel> Get(long id);
        Task<bool> Create(ArticleBindingModel model);
        Task<bool> Update(ArticleBindingModel model);
        bool Delete(long id);
        void DeleteAll(long id);
        void DeleteByService(long userId, long serviceId);
    }
}
