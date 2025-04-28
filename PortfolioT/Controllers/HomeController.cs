using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.Analysis;
using PortfolioT.Analysis.Models;
using PortfolioT.Services.GitService;
using PortfolioT.Services.GitService.RestApi.Gitea;
using PortfolioT.Services.GitService.RestApi.Gitea.Models;
using PortfolioT.Services.GitService.RestApi.Gitea.Models.Support;
using PortfolioT.Services.LibService.Models;
using PortfolioT.Services.LibService;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PortfolioT.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HomeController : ControllerBase
    {
        // GET: HomeController

        private GitService gitService;
        private LibService libService;
        public HomeController()
        {
            gitService = new GitService();
            libService = new LibService();
        }

        //[HttpGet]
        //public async Task<List<ResponseRepository>> gitea(string value)
        //{
        //    GitService gitService = new GitService();
        //    var result = await gitService.GetUserWorks(new List<ServiceData>());
        //    return result;
        //}
        //[HttpGet]
        //public async Task<IActionResult> effUsers()
        //{

        //    List<ServiceData> data1 = new List<ServiceData>()
        //    {
        //        new ServiceData()
        //        {
        //            name_service = "GitHub",
        //            data = "fortimaus"
        //        },
        //        new ServiceData()
        //        {
        //            name_service = "GitUlstu",
        //            data = "fieesr"
        //        }
        //    };
        //    List<ServiceData> data2 = new List<ServiceData>()
        //    {
        //        new ServiceData()
        //        {
        //            name_service = "GitHub",
        //            data = "fortimaus"
        //        },
        //        new ServiceData()
        //        {
        //            name_service = "GitUlstu",
        //            data = "egorvasin01"
        //        }
        //    };
        //    List<ResponseRepository> res1 = await gitService.GetUserWorks(data1);
        //    List<ResponseRepository> res2 = await gitService.GetUserWorks(data2);

        //    List<ServiceData> datas1 = new List<ServiceData>()
        //    {
        //        new ServiceData()
        //        {
        //            name_service = "GitHub",
        //            data = "Романов-2020-2025"
        //        },

        //    };
        //    List<ServiceData> datas2 = new List<ServiceData>()
        //    {
        //        new ServiceData()
        //        {
        //            name_service = "GitHub",
        //            data = "Науменко-2020-2025"
        //        },

        //    };
        //    List<Article> res3 = await libService.GetUserWorks(datas1);
        //    (string, float) user1 = ("fieesr", middle(res1, res3));
        //    (string, float) user2 = ("fieesr", middle(res2, res3.Take(3).ToList()));
        //    testresponse us1 = new testresponse() { name = "fieesr", scope=middle(res1, res3) };
        //    testresponse us2 = new testresponse() { name = "egorvasin01", scope = middle(res2, res3.Take(3).ToList()) };
        //    List<testresponse> users = new List<testresponse>() { us1, us2 };
        //    return Ok(users.OrderBy(x => x.scope));
            
        //}

        //private float middle(List<ResponseRepository> repos, List<Article> articles)
        //{
        //    float res = 0;
        //    res += repos.Average( x => (x.scope_for_decor + x.scope_for_code + x.scope_bonus));
        //    res += (float)articles.Average(x => x.scope);
        //    res /= 2;
        //    return res;
        //}


        // GET: HomController/Create

    }
}
