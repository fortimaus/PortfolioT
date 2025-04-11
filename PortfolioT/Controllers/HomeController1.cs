using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using PortfolioT.Analysis.Models;
using PortfolioT.Analysis;
using PortfolioT.Services.GitService.RestApi.GitHub;
using System.Xml;
using PortfolioT.Services.LibService;
using PortfolioT.Services.LibService.Models;

namespace PortfolioT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController1 : ControllerBase
    {
        // GET: HomeController1
        [HttpGet(Name = "github")]
        [ActionName("github")]
        public async Task<List<Article>> github(string value)
        {
            ElibUlstuParser parser = new ElibUlstuParser();
            var res = await parser.getArticles("Романов", "2010", "2025");
            Console.WriteLine(res.Count);
            return res;
        }

       
    }
}
