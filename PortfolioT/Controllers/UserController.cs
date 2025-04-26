using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;

namespace PortfolioT.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserStorage userStorage = new UserStorage();
        
        [HttpPost("register")]
        public async void Register(UserBindingModel model)
        {
            userStorage.Create(model);
        }
        [HttpGet("login")]
        public async Task<UserViewModel> Login(string login, string password)
        {
            return await userStorage.GetByLoginAndPassword(login, password);
        }
        [HttpGet("{id}")]
        public async Task<UserViewModel?> Get(long id)
        {

            return await userStorage.Get(id);
        }



        [HttpPut("update")]
        public async Task<bool?> Put(UserBindingModel model)
        {
            return await userStorage.Update(model);
        }

        [HttpPut("role")]
        public bool UpdateRole(long id, UserRole role)
        {
            return userStorage.UpdateRole(id,role);
        }

        [HttpPut("status")]
        public bool UpdateStatus(long id, UserStatus status)
        {
            return userStorage.UpdateStatus(id, status);
        }

        [HttpDelete("delete/{id}")]
        public void Delete(long id)
        {
            userStorage.Delete(id);
        }
    }
}
