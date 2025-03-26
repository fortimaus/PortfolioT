using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.Analysis;
using PortfolioT.Analysis.Models;
using PortfolioT.RestApi.Gitea;
using PortfolioT.RestApi.Gitea.Models;
using PortfolioT.RestApi.GitHub;
using PortfolioT.RestApi.GitHub.Models;
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
        public async Task<List<GiteaRepository>> gitea(string value)
        {
            HttpClient httpClient = new HttpClient();
            RestGitea restGitea = new RestGitea();
            RepositoryAnalysis analysis = new RepositoryAnalysis();
            List<(string, int)> users = new List<(string, int)> { ("VoldemarProger", 22), ("selli7", 6), ("TurnerIlya",8), ("Mars", 1) };
            foreach (var item in users)
            {
                for(int i = 0;i < 3; i++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var res = await restGitea.getInfoAsync($"https://git.is.ulstu.ru/{item.Item1}", httpClient);
                    stopwatch.Stop();
                    Console.WriteLine($"{i}: {item.Item1} TIME WORK: {stopwatch.ElapsedMilliseconds / 1000} sec");
                }
                
            }
            
            return new List<GiteaRepository>();
        }

        

        // GET: HomController/Create

    }
}
