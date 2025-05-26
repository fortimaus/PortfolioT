using PortfolioT.Analysis;
using PortfolioT.Analysis.Models;
using PortfolioT.DataBase;
using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataModels.Enums;
using PortfolioT.DataModels.Models;
using PortfolioT.Services.GitService.Models;
using PortfolioT.Services.GitService.RestApi.Gitea;
using PortfolioT.Services.GitService.RestApi.Gitea.Models;
using PortfolioT.Services.GitService.RestApi.GitHub;
using PortfolioT.Services.GitService.RestApi.GitHub.Models;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PortfolioT.Services.GitService
{
    public class GitService : IService<RepoBindingModel>
    {
        private RestGitea restGitea;
        private RestGitHub restGitHub;
        public TypeService type { get; set; } = TypeService.Repository; 
        public GitService()
        {
            restGitea = new RestGitea();
            restGitHub = new RestGitHub();

        }
        public async Task<List<RepoBindingModel>> GetUserWorks(IEnumerable<IUserService> datas)
        {
            Stopwatch stopwatch = new Stopwatch();
            long time = 0;
            stopwatch.Start();

            List<Task<List<RepoBindingModel>>> tasks = new List<Task<List<RepoBindingModel>>>();
            foreach(UserServiceBindingModel data in datas)
            {
                tasks.Add(getAndAnalisys(data));
            }
            List<RepoBindingModel>[] list_results = await Task.WhenAll(tasks);

            List<RepoBindingModel> result = new List<RepoBindingModel>();
            foreach (var list in list_results)
            {
                result.AddRange(list);
            }
            stopwatch.Stop();
            time = stopwatch.ElapsedMilliseconds / 1000;


            Console.WriteLine($"TIME : {time}");
            return result;
        }
        public async Task<List<RepoBindingModel>> GetUserWorksByService(UserServiceBindingModel data)
        {
            Stopwatch stopwatch = new Stopwatch();
            long time = 0;
            stopwatch.Start();

            List<RepoBindingModel> result = await getAndAnalisys(data);

            stopwatch.Stop();
            time = stopwatch.ElapsedMilliseconds / 1000;


            Console.WriteLine($"TIME : {time}");
            return result;
        }
        public async Task<RepoBindingModel> GetOneUserWork(long serviceId, string nameUser, string nameOwner, string repoName)
        {

            return await getAndAnalisysOne(serviceId, nameUser,nameOwner,repoName);
        }

        public async Task<bool> checkUser(long serviceId, string userName)
        {
            bool check = false;
            if (serviceId == InitServices.GitUlstu.Id)
            {
                check = await restGitea.CheckUser(userName);
            }
            else if (serviceId == InitServices.GitHub.Id)
            {
                check = await restGitHub.CheckUser(userName);

            }
            return check;
        }
        public async Task<List<AnalisisRepoBindingModel>?> GetReposInfo(long serviceId, string nameUser)
        {
            IEnumerable<IRepository> repositories;
            if (serviceId == InitServices.GitUlstu.Id)
            {
                repositories = new List<GiteaRepository>();
                repositories = await restGitea.getReposInfo(nameUser);
            }
            else if (serviceId == InitServices.GitHub.Id)
            {
                repositories = new List<GitHubRepository>();
                repositories = await restGitHub.getReposInfo(nameUser);

            }
            else
            {
                return null;
            }
            List<AnalisisRepoBindingModel> response = new List<AnalisisRepoBindingModel>();
            foreach(var repo in repositories)
            {
                response.Add(new AnalisisRepoBindingModel()
                {
                    title = repo.name,
                    link = repo.link
                });
            }
            return response;
        }
        private async Task<List<RepoBindingModel>> getAndAnalisys(UserServiceBindingModel data)
        {
            RepositoryAnalysis analysis = new RepositoryAnalysis();
            IEnumerable<IRepository> repositories;
            try
            {
                if (data.serviceId == InitServices.GitUlstu.Id)
                {
                    repositories = new List<GiteaRepository>();
                    repositories = await restGitea.getManyReposAsync(data.data);
                }
                else if (data.serviceId == InitServices.GitHub.Id)
                {
                    repositories = new List<GitHubRepository>();
                    repositories = await restGitHub.getManyReposAsync(data.data);
                }
                else
                {
                    repositories = new List<IRepository>();
                }
                List<ResponseRepository> responses = await analysis.analysisRepository(repositories, data.data);
                return responses.Select(x => x.GetRepoBindingModel(data.serviceId)).ToList();
            }
            catch
            {
                return new List<RepoBindingModel>();
            }
            
        }
        private async Task<RepoBindingModel> getAndAnalisysOne(long serviceId, string nameUser, string nameOwner, string repoName)
        {
            RepositoryAnalysis analysis = new RepositoryAnalysis();
            IRepository repository;
            if (serviceId == InitServices.GitUlstu.Id)
            {
                repository = new GiteaRepository();
                repository = await restGitea.getOneRepoAsync(nameOwner, repoName);
            }
            else if (serviceId == InitServices.GitHub.Id)
            {
                repository = new GitHubRepository();
                repository = await restGitHub.getOneRepoAsync(nameOwner, repoName);
            }
            else
            {
                return null;
            }
            ResponseRepository response = (await analysis.analysisRepository(
                new List<IRepository> {repository}, nameUser))[0];
            return response.GetRepoBindingModel(serviceId);
        }

    }
}
