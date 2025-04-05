using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.Analysis;
using PortfolioT.Analysis.Models;
using PortfolioT.Models.Request;
using PortfolioT.Services.GitService;
using PortfolioT.Services.GitService.RestApi.Gitea;
using PortfolioT.Services.GitService.RestApi.Gitea.Models;
using PortfolioT.Services.GitService.RestApi.Gitea.Models.Support;
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
            GitService gitService = new GitService();
            var result = await gitService.GetUserWorks(new List<ServiceData>());
            return result;
        }

        

        // GET: HomController/Create

    }
}
