using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.RestApi.Gitea;
using PortfolioT.RestApi.Gitea.Models;
using PortfolioT.RestApi.GitHub;
using PortfolioT.RestApi.GitHub.Models;

namespace PortfolioT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        // GET: HomeController
        

        [HttpGet(Name = "gitea")]
        public async Task<List<RestApi.Gitea.Models.Repository>> gitea(string value)
        {
            HttpClient httpClient = new HttpClient();
            RestGitea restGitea = new RestGitea();
            return await restGitea.getInfoAsync($"https://git.is.ulstu.ru/{value}", httpClient);;
        }

        

        // GET: HomController/Create

    }
}
