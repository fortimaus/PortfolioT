using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.Analysis.Models;
using PortfolioT.Models.Request;
using PortfolioT.Services.GitService;
using PortfolioT.Services.LibService;
using PortfolioT.Services.LibService.Models;
using System.Data;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PortfolioT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private LibService libService;
        public ArticleController()
        {
            libService = new LibService();
        }
        [HttpPost]
        public async Task<IActionResult> generateMany(string data)
        {
            List<ServiceData> datas = new List<ServiceData>()
            {
                new ServiceData()
                {
                    name_service = "GitHub",
                    data = data
                },
                
            };
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Article> res = await libService.GetUserWorks(datas);
            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds / 1000} sec");
            return Ok(res);
        }
        
               
    }
}
