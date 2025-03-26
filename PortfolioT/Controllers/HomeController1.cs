using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.RestApi.Gitea;
using PortfolioT.RestApi.GitHub;
using PortfolioT.RestApi.GitHub.Models;
using System.Diagnostics;

namespace PortfolioT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController1 : ControllerBase
    {
        // GET: HomeController1
        [HttpGet(Name = "github")]
        [ActionName("github")]
        public async Task<List<GitHubRepository>> github(string value)
        {
            HttpClient httpClient = new HttpClient();
            RestGitHub restGiteHub = new RestGitHub();
            List<(string, int)> users = new List<(string, int)> { ("DanilKargin", 9)};
            foreach (var item in users)
            {
                for (int i = 0; i < 3; i++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var res = await restGiteHub.getInfoAsync($"https://github.com/{item.Item1}", httpClient);
                    stopwatch.Stop();
                    Console.WriteLine($"{i}: {item.Item1} TIME WORK: {stopwatch.ElapsedMilliseconds / 1000} sec {item.Item2} repos");
                }

            }
            return new List<GitHubRepository>();
        }

       
    }
}
