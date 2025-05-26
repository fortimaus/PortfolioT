using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.BusinessLogic.Exceptions;
using PortfolioT.BusinessLogic.Logics;
using PortfolioT.Controllers.Commons;
using PortfolioT.DataBase.Commons;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.Controllers
{
    [Route("api/reposotories/")]
    [ApiController]
    public class RepoController : ControllerBase
    {
        RepoLogic repoLogic = new RepoLogic();
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Post(RepoBindingModel model)
        {
            try
            {
                return Ok(await repoLogic.Create(model));
            }
            catch (InvalidException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        // GET api/<ArticleController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult?> Get(long id)
        {
            try
            {
                return Ok(await repoLogic.Get(id));
            }
            catch (InvalidException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("user/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUsers(long id)
        {
            try
            {
                return Ok(await repoLogic.GetList(id));
            }
            catch (InvalidException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT api/<ArticleController>/5
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Put(RepoBindingModel model)
        {
            try
            {               
                return Ok(await repoLogic.Update(model));
            }
            catch (InvalidException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ArticleController>/5
        [HttpDelete("delete/{id}")]
        [Authorize]
        public IActionResult Delete(long id)
        {
            try
            {
                return Ok(repoLogic.Delete(id));
            }
            catch (InvalidException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("generate")]
        [Authorize]
        public async Task<IActionResult> generateAll(long userId)
        {
            try
            {
                return Ok(await repoLogic.generateUserAllRepo(userId));
            }
            catch (InvalidException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("generate_service")]
        [Authorize]
        public async Task<IActionResult> generateService(long userId,long serviceId)
        {
            try
            {
                return Ok(await repoLogic.generateUserRepoByService(userId,serviceId));
            }
            catch (InvalidException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("difference_many")]
        [Authorize]
        public async Task<IActionResult> DifferenceMany(long serviceId, string userName, long userId)
        {
            try
            {
                CompareUserRepoInfo myInfo = repoLogic.averageUser(userId);
                CompareUserRepoInfo otherInfo = await repoLogic.differenceManyRepo(serviceId, userName);
                CompareUserServiceResponse response = new CompareUserServiceResponse()
                {
                    myLanguageCounts = myInfo.languageCounts,
                    myDecor = myInfo.scopeDecor,
                    myCode = myInfo.scopeCode,
                    myMaintability = myInfo.scopeMaintability,
                    myReability = myInfo.scopeReability,
                    mySecurity = myInfo.scopeSecurity,

                    compareLanguageCounts = otherInfo.languageCounts,
                    compareDecor = otherInfo.scopeDecor,
                    compareCode = otherInfo.scopeCode,
                    compareMaintability = otherInfo.scopeMaintability,
                    compareReability = otherInfo.scopeReability,
                    compareSecurity = otherInfo.scopeSecurity,
                };
                return Ok(response);
            }
            catch (InvalidException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("compareAll")]
        [Authorize]
        public async Task<IActionResult> compareAll()
        {
            try
            {
                return Ok(repoLogic.averageAllUsers());
            }
            catch (InvalidException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("difference_one")]
        [Authorize]
        public async Task<IActionResult> DifferenceOne(long serviceId, string userName, string repoName)
        {
            try
            {
                return Ok(await repoLogic.differenceOneRepo(serviceId, userName, repoName));
            }
            catch (InvalidException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }



    }
}
