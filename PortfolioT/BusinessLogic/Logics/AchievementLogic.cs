using PortfolioT.BusinessLogic.Exceptions;
using PortfolioT.DataBase.Models;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;
using PortfolioT.Services;
using PortfolioT.Services.GitService;
using PortfolioT.Services.LibService;

namespace PortfolioT.BusinessLogic.Logics
{
    public class AchievementLogic : IAchievementLogic
    {
        AchievementStorage achievementStorage;
        UserServiceStorage serviceStorage;

        RepoStorage repoStorage;
        ArticleStorage articleStorage;

        GitService gitService;
        LibService libService;

        public AchievementLogic()
        {
            achievementStorage = new AchievementStorage();
            serviceStorage = new UserServiceStorage();

            repoStorage = new RepoStorage();
            articleStorage = new ArticleStorage();

            gitService = new GitService();
            libService = new LibService();
        }
        public async Task<bool> Create(AchievementBindingModel model)
        {
            try
            {
                validate(model);
                return await achievementStorage.Create(model);
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
                return achievementStorage.Delete(id);
            }
            catch
            {
                throw;
            }

        }

        public async Task<AchievementViewModel?> Get(long id)
        {
            try{
                return await achievementStorage.Get(id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<AchievementViewModel>> GetList(long id)
        {
            try
            {
                return await achievementStorage.GetList(id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(AchievementBindingModel model)
        {
            try
            {
                validate(model);
                return await achievementStorage.Update(model);
            }
            catch
            {
                throw;
            }
            
        }

        private void validate(AchievementBindingModel model)
        {
            if (model.title.Length == 0)
                throw new InvalidException("Название не должно быть пустым");
            if (model.description.Length == 0)
                throw new InvalidException("Описание не должно быть пустым");
        }

        public async Task<bool> getAllUserWorks(long id)
        {
            try
            {
                List<UserServiceViewModel> datas = serviceStorage.GetUserList(id);
                Dictionary<TypeService, List<UserServiceViewModel>> user_services = new Dictionary<TypeService, List<UserServiceViewModel>>();
                foreach (var data in datas)
                {
                    if (!user_services.ContainsKey(data.type))
                        user_services.Add(data.type, new List<UserServiceViewModel>() { data });
                    else
                        user_services[data.type].Add(data);
                }
                List<Task<bool>> tasks = new List<Task<bool>>();
                foreach (var data in user_services)
                {
                    switch (data.Key)
                    {
                        case TypeService.Repository:
                            tasks.Add(SaveRepositories(data.Value,id));
                            break;
                        case TypeService.Article:
                            tasks.Add(SaveArticle(data.Value,id));
                            break;
                    }
                    ;
                }
                await Task.WhenAll(tasks);
                return true;
            }
            catch
            {
                return false;
                throw;
                
            }
        }
        private async Task<bool> SaveRepositories(List<UserServiceViewModel> datas, long id)
        {
            repoStorage.DeleteAll(id);
            List<UserServiceBindingModel> models = datas.Select(x => new UserServiceBindingModel()
            {
                userId = x.userId,
                serviceId = x.serviceId,
                data = x.data
            }).ToList();
            List<RepoBindingModel> repos = await gitService.GetUserWorks(models);
            foreach (var repo in repos)
            {
                repo.userId = id;
                await repoStorage.Create(repo);
            }
            return true;
        }
        private async Task<bool> SaveArticle(List<UserServiceViewModel> datas, long id)
        {
            articleStorage.DeleteAll(id);
            List<UserServiceBindingModel> models = datas.Select(x => new UserServiceBindingModel()
            {
                userId = x.userId,
                serviceId = x.serviceId,
                data = x.data
            }).ToList();
            List<ArticleBindingModel> articles = await libService.GetUserWorks(models);
            foreach (var article in articles)
            {
                article.userId = id;
                await articleStorage.Create(article);
            }
            return true;
        }
    }
}
