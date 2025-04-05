using PortfolioT.Analysis;
using PortfolioT.Analysis.Models;
using PortfolioT.Models.Request;
using PortfolioT.Services.GitService.Models;
using PortfolioT.Services.GitService.RestApi.Gitea;
using PortfolioT.Services.GitService.RestApi.Gitea.Models;
using PortfolioT.Services.GitService.RestApi.GitHub;
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
            
            var repos = await restGitea.getManyReposAsync("stanislav");
            
            stopwatch.Stop();
            timeGet = stopwatch.ElapsedMilliseconds / 1000;
            stopwatch.Reset();
            stopwatch.Start();
            
            var res = analysis.analysisRepository(repos, "stanislav"); ;
            
            stopwatch.Stop();

            timeAn = stopwatch.ElapsedMilliseconds / 1000;
            Console.WriteLine($"TIME Get: {timeGet} sec TIME Analisys: {timeAn} sec");
            return res;
        }
    }
}
