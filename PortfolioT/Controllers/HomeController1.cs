using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.RestApi.GitHub;

namespace PortfolioT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController1 : ControllerBase
    {
        // GET: HomeController1
        [HttpGet(Name = "github")]
        public async Task<List<RestApi.GitHub.Models.Repository>> github(string value)
        {
            HttpClient httpClient = new HttpClient();
            RestGitHub restGiteHub = new RestGitHub();
            return await restGiteHub.getInfoAsync($"https://github.com/{value}", httpClient); ;
        }

       
    }
}
