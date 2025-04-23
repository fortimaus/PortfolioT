using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.Analysis.Models;
using PortfolioT.DataBase.Models;
using PortfolioT.Models.Request;
using PortfolioT.Services.GitService;

namespace PortfolioT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private GitService gitService;
        public RepositoryController()
        {
            gitService = new GitService();
        }
        [HttpPost]
        public async Task<IActionResult> generateMany(string ulstu, string github)
        {
            List<ServiceData> datas = new List<ServiceData>()
            {
                new ServiceData()
                {
                    name_service = "GitHub",
                    data = github
                },
                new ServiceData()
                {
                    name_service = "GitUlstu",
                    data = ulstu
                }
            };
            List<ResponseRepository> res = await gitService.GetUserWorks(datas);
            return Ok(res);
        }
        
        [HttpPost]
        public async Task<IActionResult> generateOne(string service, string nameUser, string nameOwner, string repoName)
        {
            ResponseRepository res = await gitService.GetOneWork(service,nameUser,nameOwner,repoName);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> test()
        {
            List<List<ServiceData>> list = new List<List<ServiceData>>()
            {
                new List<ServiceData>()
                {
                    new ServiceData()
                {
                    name_service = "GitHub",
                    data = "fortimaus"
                },
                new ServiceData()
                {
                    name_service = "GitUlstu",
                    data = "VoldemarProger"
                }
                },

                new List<ServiceData>()
                {
                    new ServiceData()
                {
                    name_service = "GitHub",
                    data = "fortimaus"
                },
                new ServiceData()
                {
                    name_service = "GitUlstu",
                    data = "TurnerIlya"
                }
                },
                new List<ServiceData>()
                {
                    new ServiceData()
                {
                    name_service = "GitHub",
                    data = "fortimaus"
                },
                new ServiceData()
                {
                    name_service = "GitUlstu",
                    data = "Sosees04ka"
                }
                },
                new List<ServiceData>()
                {
                    new ServiceData()
                {
                    name_service = "GitHub",
                    data = "fortimaus"
                },
                new ServiceData()
                {
                    name_service = "GitUlstu",
                    data = "maxKarme"
                }
                },
                new List<ServiceData>()
                {
                    new ServiceData()
                {
                    name_service = "GitHub",
                    data = "fortimaus"
                },
                new ServiceData()
                {
                    name_service = "GitUlstu",
                    data = "Andrey_Abazov"
                }
                },
                new List<ServiceData>()
                {
                    new ServiceData()
                {
                    name_service = "GitHub",
                    data = "fortimaus"
                },
                new ServiceData()
                {
                    name_service = "GitUlstu",
                    data = "maxnes3"
                }
                },

            };
            List<ResponseRepository> res = new List<ResponseRepository>();
            foreach (var item in list)
            {
                Console.WriteLine(item[1].data);
                res.AddRange(await gitService.GetUserWorks(item));
                await Task.Delay(3000);
            }
            return Ok(res);
        }
    }
}
