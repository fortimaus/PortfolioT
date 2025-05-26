using PortfolioT.BusinessLogic.Exceptions;
using PortfolioT.DataBase.Models;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;
using PortfolioT.Services.LibService;

namespace PortfolioT.BusinessLogic.Logics
{
    public class ArticleLogic : IArticleLogic
    {
        ArticleStorage articleStorage;
        LibService libService;
        UserServiceStorage serviceStorage;
        UserStorage userStorage;
        public ArticleLogic()
        {
            articleStorage = new ArticleStorage();
            serviceStorage = new UserServiceStorage();
            userStorage = new UserStorage();
            libService = new LibService();
        }
        public async Task<bool> Create(ArticleBindingModel model)
        {
            try
            {
                validate(model); 
                return await articleStorage.Create(model);
            }
            catch
            {
                throw;
            }
            
        }

        public bool Delete(long id)
        {
            try
            {
                long userId = articleStorage.GetUser(id);
                articleStorage.Delete(id);
                userStorage.updateRating(userId);
                return true;
            }
            catch
            {
                throw;
            }

        }

        public async Task<ArticleViewModel?> Get(long id)
        {
            try
            {
                return await articleStorage.Get(id);

            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ArticleViewModel>> GetList(long id)
        {
            try
            {
                return await articleStorage.GetList(id);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(ArticleBindingModel model)
        {
            try
            {
                validate(model) ;
                return await articleStorage.Update(model);
            }
            catch
            {
                throw;
            }

        }

        private void validate(ArticleBindingModel model)
        {
            if (model.title.Length == 0)
                throw new InvalidException("Название не должно быть пустым");
        }

        public async Task<bool> generateUserAllArticle(long userId)
        {
            try
            {
                articleStorage.DeleteAll(userId);
                List<UserServiceViewModel> datas = serviceStorage.GetUserListByService(userId, TypeService.Article);
                List<UserServiceBindingModel> models = datas.Select(x => new UserServiceBindingModel()
                {
                    userId = x.userId,
                    serviceId = x.serviceId,
                    data = x.data
                }).ToList();
                List<ArticleBindingModel> articles = await libService.GetUserWorks(models);
                foreach (var article in articles)
                {
                    article.userId = userId;
                    await articleStorage.Create(article);
                }
                userStorage.updateRating(userId);
                return true;
            }
            catch
            {
                return false;
                throw;
            }
        }
        public async Task<bool> generateUserArticleByService(long userId, long serviceId)
        {

            try
            {
                articleStorage.DeleteByService(userId,serviceId);
                UserServiceViewModel? data = serviceStorage.GetUser(userId, serviceId);
                if (data == null)
                    return false;
                List<ArticleBindingModel> articles = await libService.GetArticlesByService(data);
                foreach (var article in articles)
                {
                    article.userId = userId;
                    await articleStorage.Create(article);
                }
                userStorage.updateRating(userId);
                return true;
            }
            catch
            {
                return false;
                throw;
            }
        }
    }
}
