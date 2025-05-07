using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.BusinessLogic.Exceptions;
using PortfolioT.BusinessLogic.Logics;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;

namespace PortfolioT.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        UserServiceLogic userServiceLogic = new UserServiceLogic();
        ServiceLogic serviceLogic = new ServiceLogic();

        [HttpPost("create")]
        [Authorize]
        public IActionResult Post(UserServiceBindingModel model)
        {
            try
            {
                return Ok(userServiceLogic.Create(model));
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
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetByUsers(long id)
        {
            try
            {
                return Ok(userServiceLogic.GetUserList(id));
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

        [HttpGet]
        [Authorize]
        public IActionResult GetList()
        {
            try
            {
                return Ok(serviceLogic.getList());
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

        [HttpGet("type")]
        [Authorize]
        public IActionResult GetByType(TypeService type)
        {
            try
            {
                return Ok(serviceLogic.getList(type));
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

        [HttpPut("update")]
        [Authorize]
        public IActionResult Update(UserServiceBindingModel model)
        {
            try
            {
                return Ok(userServiceLogic.Update(model));
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

        [HttpDelete("delete")]
        [Authorize]
        public IActionResult Delete(UserServiceBindingModel model)
        {
            try
            {
                return Ok(userServiceLogic.Delete(model));
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
