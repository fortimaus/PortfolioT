using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.BusinessLogic.Exceptions;
using PortfolioT.BusinessLogic.Logics;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        ArticleLogic articleLogic = new ArticleLogic();
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Post(ArticleBindingModel model)
        {
            try
            {

                return Ok(await articleLogic.Create(model));
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
                return Ok(await articleLogic.Get(id));
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
        public async Task<IActionResult> Generate(long id)
        {
            try
            {
                return Ok(await articleLogic.generateUserAllArticle(id));
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
        public async Task<IActionResult> GenerateByService(long userId, long serviceId)
        {
            try
            {
                return Ok(await articleLogic.generateUserArticleByService(userId,serviceId));
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
                return Ok(await articleLogic.GetList(id));
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
        public async Task<IActionResult> Put(ArticleBindingModel model)
        {
            try
            {
                return Ok(await articleLogic.Update(model));
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
                return Ok(articleLogic.Delete(id));
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
    }
}
