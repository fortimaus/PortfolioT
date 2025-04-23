using PortfolioT.Analysis;
using PortfolioT.Analysis.Models;
using PortfolioT.DataBase.Models;
using PortfolioT.Models.Request;
using PortfolioT.Services.GitService.Models;
using PortfolioT.Services.GitService.RestApi.Gitea;
using PortfolioT.Services.GitService.RestApi.Gitea.Models;
using PortfolioT.Services.GitService.RestApi.GitHub;
using PortfolioT.Services.GitService.RestApi.GitHub.Models;
using System.Diagnostics;

namespace PortfolioT.Services.GitService
{
    public class GitService : IService<ResponseRepository>
    {
        private RestGitea restGitea;
        private RestGitHub restGitHub;
        private RepositoryAnalysis analysis;
        public GitService()
        {
            restGitea = new RestGitea();
            restGitHub = new RestGitHub();
            analysis = new RepositoryAnalysis();
        }

        public async Task<List<ResponseRepository>> GetUserWorks(List<ServiceData> datas)
        {
            Stopwatch stopwatch = new Stopwatch();
            long timeGet = 0;
            long timeAn = 0;
            stopwatch.Start();
            //Console.WriteLine("Repo \t\t\t commits \t\t\t time");
            List<GiteaRepository> giteaRepositories = new List<GiteaRepository>();
            List<GitHubRepository> gitHubRepositories = new List<GitHubRepository>();

            Task<List<GitHubRepository>> gitHubTask = null;
            Task<List<GiteaRepository>> gitUlstuTask = null;
            foreach (var data in datas)
            {

                if (data.name_service.Equals("GitHub"))
                    gitHubTask = restGitHub.getManyReposAsync(data.data);
                else if (data.name_service.Equals("GitUlstu"))
                    gitUlstuTask = restGitea.getManyReposAsync(data.data);

            }
            giteaRepositories = await gitUlstuTask;
            gitHubRepositories = await gitHubTask;

            
            stopwatch.Stop();
            timeGet = stopwatch.ElapsedMilliseconds / 1000;
            stopwatch.Reset();
            stopwatch.Start();

            Task<List<ResponseRepository>> gitHubAnTask = null;
            Task<List<ResponseRepository>> gitUlstuAnTask = null;

            List<ResponseRepository> res = new List<ResponseRepository>();
            
            gitHubAnTask = analysis.analysisRepository(gitHubRepositories, "fortimaus");
            gitUlstuAnTask = analysis.analysisRepository(giteaRepositories, "VoldemarProger");
            res.AddRange(await gitHubAnTask);
            res.AddRange(await gitUlstuAnTask);
            stopwatch.Stop();

            timeAn = stopwatch.ElapsedMilliseconds / 1000;
            Console.WriteLine($"TIME Get: {timeGet} sec TIME Analisys: {timeAn} sec");
            return res;
        }
        public async Task<ResponseRepository> GetOneWork(string service, string nameUser, string nameOwner, string repoName)
        {
            IRepository repo = null;
            if (service.Equals("GitHub"))
                repo = await restGitHub.getOneRepoAsync(nameOwner, repoName);
            else if (service.Equals("GitUlstu"))
                repo = await restGitea.getOneRepoAsync(nameOwner, repoName);
            var res = await analysis.analysisRepository(new List<IRepository>() {repo}, nameUser);
            return res[0];
        }
    }
}
