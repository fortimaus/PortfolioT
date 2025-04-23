using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.DataBase.Models;
using System.Runtime.CompilerServices;

namespace PortfolioT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static List<User> users = new List<User>()
        {
        //    new User()
        //{
        //    id = 1,
        //        login = "voldemar",
        //        password = "12345678",
        //        email = "vladimirMorozov.73@yandex.ru",
        //        links = new List<LinkService>()
        //        {
        //            new LinkService()
        //            {
        //                service = "GitUlstu",
        //                data = "VoldemarProger"
        //            },
        //            new LinkService()
        //            {
        //                service = "GitHub",
        //                data = "fortimaus"
        //            },
        //            new LinkService()
        //            {
        //                service = "ElibUlstu",
        //                data = "Науменко А&2020&2025"
        //            },
        //        }
        //    },
        //    new User()
        //{
        //    id = 2,
        //        login = "returner",
        //        password = "12345678",
        //        email = "returner@yandex.ru",
        //        scope = 80
              
        //    },
        //    new User()
        //{
        //    id = 3,
        //        login = "kar_am_ushko",
        //        password = "12345678",
        //        email = "kar_am_ushko@yandex.ru",
        //        scope = 75
        //    }
    };
        private int id = 4;
        [HttpGet]
        public IActionResult login(string name, string password)
        {
            User? user = users.SingleOrDefault(x => x.login.Equals(name) && x.password.Equals(password));
            if (user != null)
                return Ok(user);
            else
                return BadRequest("User not exist");
        }
        //[HttpPost]
        //public IActionResult register(string login, string password, string email)
        //{
        //    User? user = users.SingleOrDefault(x => x.login.Equals(login));
        //    if (user == null)
        //    {
        //        User newUser = new User()
        //        {
        //            id = id,
        //            login = login,
        //            password = password,
        //            email = email
        //        };
        //        id++;
        //        users.Add(newUser);
        //        return Ok(newUser);
        //    } 
        //    else
        //        return BadRequest("User with that login already exist");
        //}
        //[HttpPut]
        //public IActionResult update(int id, string newLogin, string newPassword, string newEmail, string about)
        //{
        //    User? user = users.SingleOrDefault(x => x.id == id );
            
        //    if (user != null)
        //    {
        //        user.login = newLogin;
        //        user.password = newPassword;
        //        user.email = newEmail;
        //        user.about = about;

        //        int ind = users.IndexOf(user);
        //        users[ind] = user;

        //        return Ok(user);
        //    }
        //    else
        //        return BadRequest("User not exist");
        //}
        //[HttpPut]
        //public IActionResult updateLink(int id, string service, string data)
        //{
        //    User? user = users.SingleOrDefault(x => x.id == id);

        //    if (user != null)
        //    {
        //        user.links.Add(new LinkService() { service = service, data = data });
        //        int ind = users.IndexOf(user);
        //        users[ind] = user;

        //        return Ok(user);
        //    }
        //    else
        //        return BadRequest("User not exist");
        //}
        //[HttpGet]
        //public IActionResult get(int id)
        //{
        //    User? user = users.SingleOrDefault(x => x.id == id);

        //    if (user != null)
        //    {
        //       return Ok(user);
        //    }
        //    else
        //        return BadRequest("User not exist");
        //}
        //[HttpGet]
        //public IActionResult getAll()
        //{
        //    return Ok(users.OrderBy(x => x.scope));
        //}
    }
}
