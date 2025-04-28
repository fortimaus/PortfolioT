using Microsoft.AspNetCore.Mvc;
using PortfolioT.BusinessLogic.Exceptions;
using PortfolioT.BusinessLogic.Logics;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortfolioT.Controllers
{
    [Route("api/achievements")]
    [ApiController]
    public class AchievementController : ControllerBase
    {

        AchievementLogic achievementLogic = new AchievementLogic();
        // POST api/<AchievementController>
        [HttpPost("create")]
        public async Task<IActionResult> Post(AchievementBindingModel model)
        {
            try
            {
                testImage test = new testImage();
                byte[] prev = await test.preview();
                byte[] im1 = await test.image1();
                byte[] im2 = await test.image2();
                model.preview = prev;
                model.images = new List<(long, byte[])>();
                model.images.Add((-1, im1));
                model.images.Add((-1, im2));
                return Ok(await achievementLogic.Create(model));
            }
            catch(InvalidException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return ValidationProblem(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        // GET api/<AchievementController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult?> Get(long id)
        {
            try
            {
                return Ok(await achievementLogic.Get(id));
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
        public async Task<IActionResult> Generate(long id)
        {
            try
            {
                return Ok(await achievementLogic.getAllUserWorks(id));
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
        public async Task<IActionResult> GetUsers(long id)
        {
            try
            {
                return Ok(await achievementLogic.GetList(id));
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

        // PUT api/<AchievementController>/5
        [HttpPut("update")]
        public async Task<IActionResult> Put(AchievementBindingModel model)
        {
            try
            {
                testImage test = new testImage();
                byte[] prev = await test.preview();
                byte[] im1 = await test.image1();
                byte[] im2 = await test.image2();
                model.preview = im2;
                model.images = new List<(long, byte[])>();
                model.images.Add((-1, im1));
                return Ok(await achievementLogic.Update(model));
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

        // DELETE api/<AchievementController>/5
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                return Ok(achievementLogic.Delete(id));
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
