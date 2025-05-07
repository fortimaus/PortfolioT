using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PortfolioT.BusinessLogic.Exceptions;
using PortfolioT.BusinessLogic.Logics;
using PortfolioT.Controllers.Commons;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;
using PortfolioT.MailWorker;
using System.Data;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace PortfolioT.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserLogic userLogic = new UserLogic();
        UserCommentLogic commentLogic = new UserCommentLogic();
        
        

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserBindingModel model)
        {
            try
            {
                return Ok(await userLogic.Create(model));
            }
            catch (BusyUserException ex)
            {
                return ValidationProblem(ex.Message);
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
        [HttpPost("confirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> confirmEmail(long id, string code)
        {
            try
            {
                if (userLogic.CheckCode(id, code))
                    userLogic.UpdateRole(id, UserRole.User);
                else
                    return ValidationProblem();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("updateEmail")]
        [Authorize]
        public async Task<IActionResult> updateEmail(long id, string email)
        {
            try
            {
                return Ok(userLogic.UpdateCodeForEmail(id, email));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("confirmNewEmail")]
        [Authorize]
        public async Task<IActionResult> confirmNewEmail(long id,string code,string email)
        {
            try
            {
                if (userLogic.CheckCode(id, code))
                    userLogic.UpdateEmail(id, email);
                else
                    return ValidationProblem();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string login, string password)
        {
            try
            {
                var user = await userLogic.FindByLoginAndPassword(login, password);
                var claims = new List<Claim> 
                    {
                    new Claim("Id",$"{user.Id}"),
                    new Claim(ClaimTypes.Role, $"{Enum.GetName(typeof(UserRole), user.role)}"),
                    };
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "MyAuthServer",
                    audience: "MyAuthClient",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new LoginResponse()
                {
                    token = tokenString,
                    user = user
                });
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
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                return Ok(await userLogic.Get(id));
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
        public async Task<IActionResult> Put(UserBindingModel model)
        {
            try
            {
                return Ok(await userLogic.Update(model));
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

        [HttpPut("role")]
        [Authorize(Roles = $"Admin")]
        public IActionResult UpdateRole(long id, UserRole role)
        {
            try
            {
                return Ok(userLogic.UpdateRole(id, role));
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

        [HttpPut("status")]
        [Authorize(Roles = $"Admin,Moderator")]
        public IActionResult UpdateStatus(long id, UserStatus status)
        {
            try
            {
                return Ok(userLogic.UpdateStatus(id, status));
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

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(long id)
        {
            try
            {
                return Ok(userLogic.Delete(id));
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
        [HttpPost("comments")]
        [Authorize(Roles = $"Admin,Moderator")]
        public IActionResult CreateComment(UserCommentBindingModel model)
        {
            try
            {
                return Ok(commentLogic.Create(model));
            }
            catch (BusyUserException ex)
            {
                return ValidationProblem(ex.Message);
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
        [HttpGet("comments/{id}")]
        [Authorize]
        public IActionResult Comments(long id)
        {
            try
            {
                return Ok(commentLogic.UserComments(id));
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
