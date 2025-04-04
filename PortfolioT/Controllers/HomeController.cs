using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.Analysis;
using PortfolioT.Analysis.Models;
using PortfolioT.RestApi.Gitea;
using PortfolioT.RestApi.Gitea.Models;
using PortfolioT.RestApi.Gitea.Models.Support;
using PortfolioT.RestApi.GitHub;
using PortfolioT.RestApi.GitHub.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace PortfolioT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        // GET: HomeController
        

        [HttpGet(Name = "gitea")]
        [ActionName("gitea")]
        public async Task<List<ResponseRepository>> gitea(string value)
        {
            HttpClient httpClient = new HttpClient();
            RestGitea restGitea = new RestGitea();
            RepositoryAnalysis analysis = new RepositoryAnalysis();
            List<(string, int)> users = new List<(string, int)> { ("VoldemarProger", 22)};
            List<ResponseRepository> result = new List<ResponseRepository>();
            foreach (var item in users)
            {
                for(int i = 0;i < 1; i++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    long timeWorkGet = 0;
                    long timeWorkAnalisys = 0;
                    
                    stopwatch.Start();
                    var res = await restGitea.getInfoAsync($"https://git.is.ulstu.ru/{item.Item1}", httpClient);
                    stopwatch.Stop();                 
                    timeWorkGet = stopwatch.ElapsedMilliseconds / 1000;
                    
                    stopwatch.Reset();
                    stopwatch.Start();
                    result = analysis.analysisRepository<GiteaRepository, GiteaCommit, GiteaPullRequest, GiteaCommitFile>(res, item.Item1);
                    stopwatch.Stop();
                    timeWorkAnalisys = stopwatch.ElapsedMilliseconds / 1000;

                    Console.WriteLine($"{i}: {item.Item1} TIME Get: {timeWorkGet} sec TIME Analisys: {timeWorkAnalisys} sec");
                }
                
            }
            
            return result;
        }

        

        // GET: HomController/Create

    }
}
