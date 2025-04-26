using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        UserServiceStorage userServiceStorage = new UserServiceStorage();
        ServiceStorage serviceStorage = new ServiceStorage();
        [HttpPost("create")]
        public void Post(UserServiceBindingModel model)
        {
            userServiceStorage.Create(model);
        }
        [HttpGet("{id}")]
        public List<UserServiceViewModel>? GetByUsers(long id)
        {
            return userServiceStorage.GetUserList(id);
        }

        [HttpGet]
        public List<ServiceViewModel>? GetList()
        {
            return serviceStorage.getList();
        }

        [HttpDelete("delete")]
        public void Delete(UserServiceBindingModel model)
        {
            userServiceStorage.Delete(model);
        }
    }
}
