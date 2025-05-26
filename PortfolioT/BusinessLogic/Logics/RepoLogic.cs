using PortfolioT.BusinessLogic.Exceptions;
using PortfolioT.DataBase;
using PortfolioT.DataBase.Commons;
using PortfolioT.DataBase.Models;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;
using PortfolioT.DataModels.Models;
using PortfolioT.Services.GitService;
using PortfolioT.Services.GitService.RestApi.Gitea;
using PortfolioT.Services.GitService.RestApi.GitHub;
using PortfolioT.Services.LibService;
using System.Collections.Generic;

namespace PortfolioT.BusinessLogic.Logics
{
    public class RepoLogic : IRepoLogic
    {
        RepoStorage repoStorage;
        UserServiceStorage serviceStorage;
        UserStorage userStorage;
        GitService gitService;

        AnalisysUserStorage analisysUserStorage;
        AnalisisRepoStorage analisisRepoStorage;
        public RepoLogic()
        {
            repoStorage = new RepoStorage();
            serviceStorage = new UserServiceStorage();
            gitService = new GitService();
            userStorage = new UserStorage();

            analisisRepoStorage = new AnalisisRepoStorage();
            analisysUserStorage = new AnalisysUserStorage();
        }
        public async Task<bool> Create(RepoBindingModel model)
        {
            try
            {
                validate(model);
                return await repoStorage.Create(model);
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
                long userId = repoStorage.GetUser(id);
                repoStorage.Delete(id);
                userStorage.updateRating(userId);
                return true;
            }
            catch
            {
                throw;
            }

        }

        public async Task<RepoViewModel?> Get(long id)
        {
            try
            {
                return await repoStorage.Get(id);

            }
            catch
            {
                throw;
            }
        }

        public async Task<List<RepoViewModel>> GetList(long id)
        {
            try
            {
                return await repoStorage.GetList(id);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Update(RepoBindingModel model)
        {
            try
            {
                validate(model);
                return await repoStorage.Update(model);
            }
            catch
            {
                throw;
            }
        }

        private void validate(RepoBindingModel model)
        {
            if (model.title.Length == 0)
                throw new InvalidException("Название не должно быть пустым");
        }

        public async Task<List<RepoViewModel>> generateUserAllRepo(long userId)
        {
            try
            {
                repoStorage.DeleteAll(userId);
                List<UserServiceViewModel> datas = serviceStorage.GetUserListByService(userId, TypeService.Repository);
                List<UserServiceBindingModel> models = datas.Select(x => new UserServiceBindingModel()
                {
                    userId = x.userId,
                    serviceId = x.serviceId,
                    data = x.data
                }).ToList();
                List<RepoBindingModel> repos = await gitService.GetUserWorks(models);
                foreach (var repo in repos)
                {
                    repo.userId = userId;
                    await repoStorage.Create(repo);
                }
                userStorage.updateRating(userId);
                return await repoStorage.GetList(userId);
            }
            catch
            {
                return new List<RepoViewModel>();
                throw;
            }
        }
        public async Task<List<RepoViewModel>> generateUserRepoByService(long userId, long serviceId)
        {

            try
            {
                repoStorage.DeleteByService(userId, serviceId);
                UserServiceViewModel? data = serviceStorage.GetUser(userId, serviceId);
                if (data == null)
                    return new List<RepoViewModel>();
                List<RepoBindingModel> repos = await gitService.GetUserWorksByService(new UserServiceBindingModel()
                {
                    serviceId = data.serviceId,
                    userId = data.userId,
                    data = data.data
                });
                foreach (var repo in repos)
                {
                    repo.userId = userId;
                    await repoStorage.Create(repo);
                }
                userStorage.updateRating(userId);
                return await repoStorage.GetList(userId);
            }
            catch
            {
                return new List<RepoViewModel>();
                throw;
            }
        }

        public async Task<AnalisisRepoViewModel> differenceOneRepo(long serviceId, string userName, string repoName)
        {
            try
            {
                if (!await gitService.checkUser(serviceId, userName))
                    throw new UnexistedUserException("Не существующий пользователь");
                AnalisisUser user = analisysUserStorage.TryGet(new AnalisysUserBindingModel()
                {
                    serviceId = serviceId,
                    name = userName,
                    link = createLinkUser(serviceId, userName)
                });

                var analisys = analisisRepoStorage.GetOne(user.Id, repoName);
                if (analisys == null || (DateTime.Now - analisys.date).TotalDays >= 14)
                {
                    if (analisys != null)
                        analisisRepoStorage.Delete(analisys.id);
                    var repo = await gitService.GetOneUserWork(serviceId, userName, userName, repoName);
                    analisisRepoStorage.Create(new AnalisisRepoBindingModel()
                    {
                        title = repo.title,
                        language = repo.language,
                        serviceId = serviceId,
                        userId = user.Id,
                        link = repo.link,
                        scope_code = repo.scope_code,
                        scope_decor = repo.scope_decor,
                        scope_maintability = repo.scope_maintability,
                        scope_reability = repo.scope_reability,
                        scope_security = repo.scope_security
                    });
                }
                return analisisRepoStorage.GetOne(user.Id, repoName);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                throw;
            }
            
        }

        public CompareUserRepoInfo averageUser(long id)
        {
            return repoStorage.GetAverage(id);
        }
        public CompareUserRepoInfo averageAllUsers()
        {
            return repoStorage.GetAverageAllUsers();
        }
        public async Task<CompareUserRepoInfo> differenceManyRepo(long serviceId, string userName)
        {
            try
            {
                if (!await gitService.checkUser(serviceId, userName))
                    throw new UnexistedUserException("Не существующий пользователь");
                AnalisisUser user = analisysUserStorage.TryGet(new AnalisysUserBindingModel()
                {
                    serviceId = serviceId,
                    name = userName,
                    link = createLinkUser(serviceId, userName)
                });
                List<IAnalisisRepo> update_repos = new();

                var OldRepos = analisisRepoStorage.GetList(user.Id);
                var NewRepos = await gitService.GetReposInfo(serviceId, userName);
                if (OldRepos == null && NewRepos == null)
                    return null;

                update_repos.AddRange(
                    NewRepos.Where(x =>
                        OldRepos.FirstOrDefault(y =>
                            y.title.Equals(x.title)) == null).ToList()
                    );

                List<AnalisisRepoViewModel> oldestRepos = OldRepos
                    .Where(x => (DateTime.Now - x.date).TotalDays >= 14)
                    .ToList();

                update_repos.AddRange(oldestRepos);
                analisisRepoStorage.DeleteMany(oldestRepos.Select(x => x.id).ToList());
                List<RepoBindingModel> results = await getManyDifferenceAnalisis(update_repos, serviceId, userName);
                analisisRepoStorage.CreateList(
                    results.Select(x => new AnalisisRepoBindingModel()
                    {
                        title = x.title,
                        link = x.link,
                        language = x.language,
                        userId = user.Id,
                        serviceId = serviceId,
                        scope_code = x.scope_code,
                        scope_decor = x.scope_decor,
                        scope_maintability = x.scope_maintability,
                        scope_reability = x.scope_reability,
                        scope_security = x.scope_security,

                    }).ToList()
                    );

                return analisisRepoStorage.GetAverage(user.Id);
            }
            catch
            {
                throw;
            }
            
        }
        private string createLinkUser(long serviceId, string link)
        {
            if (serviceId == InitServices.GitUlstu.Id)
                return $@"{RestGitea.URL}{link}";
            if(serviceId == InitServices.GitHub.Id)
                return $@"{RestGitHub.URL}{link}";
            return "";
        }
        private async Task<List<RepoBindingModel>> getManyDifferenceAnalisis(IEnumerable<IAnalisisRepo> elements, long serviceId, string name)
        {
            int bath = 3;
            List<RepoBindingModel> results = new List<RepoBindingModel>();
            int pages = (int)Math.Ceiling((float)elements.Count()/ bath);
            for (int i = 0; i < pages; i++)
            {
                var tasks = elements
                    .Skip(i * bath)
                    .Take(bath).Select(x => gitService.GetOneUserWork(serviceId,name,name,x.title));
                results.AddRange(await Task.WhenAll(tasks));
            }
            return results;
        }
    }
}
