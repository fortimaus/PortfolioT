using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.RestApi.Gitea;
using PortfolioT.RestApi.Gitea.Models.Support;
using PortfolioT.RestApi.Gitea.Models;
using PortfolioT.RestApi.GitHub;
using PortfolioT.RestApi.GitHub.Models;
using System.Diagnostics;
using PortfolioT.Analysis.Models;
using PortfolioT.Analysis;
using PortfolioT.RestApi.GitHub.Models.Support;

namespace PortfolioT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController1 : ControllerBase
    {
        // GET: HomeController1
        [HttpGet(Name = "github")]
        [ActionName("github")]
        public async Task<List<ResponseRepository>> github(string value)
        {
            HttpClient httpClient = new HttpClient();
            RestGitHub restGiteHub = new RestGitHub();
            RepositoryAnalysis analysis = new RepositoryAnalysis();
            List<(string, int)> users = new List<(string, int)> { ("viltskaa", 50)};
            List<ResponseRepository> result = new List<ResponseRepository>();
            //foreach (var item in users)
            //{
            //    for (int i = 0; i < 1; i++)
            //    {
            //        Stopwatch stopwatch = new Stopwatch();
            //        long timeWorkGet = 0;
            //        long timeWorkAnalisys = 0;

            //        stopwatch.Start();
            //        var res = await restGiteHub.getInfoAsync($"https://github.com/{item.Item1}");
            //        stopwatch.Stop();
            //        timeWorkGet = stopwatch.ElapsedMilliseconds / 1000;

            //        stopwatch.Reset();
            //        stopwatch.Start();
            //        result = analysis.analysisRepository<GitHubRepository, GitHubCommit, GitHubPullRequest, GitHubCommitFile>(res, item.Item1);
            //        stopwatch.Stop();
            //        timeWorkAnalisys = stopwatch.ElapsedMilliseconds / 1000;

            //        Console.WriteLine($"{i}: {item.Item1} TIME Get: {timeWorkGet} sec TIME Analisys: {timeWorkAnalisys} sec");

            //    }

            //}
            Stopwatch stopwatch = new Stopwatch();
            await restGiteHub.GetZip("viltskaa", "Consul");
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds / 1000);
            return result;
        }

       
    }
}
