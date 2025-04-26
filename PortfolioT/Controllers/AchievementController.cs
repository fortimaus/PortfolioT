using Microsoft.AspNetCore.Mvc;
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
        AchievementStorage achievementStorage = new AchievementStorage();
        // POST api/<AchievementController>
        [HttpPost("create")]
        public async void Post(AchievementBindingModel model)
        {
            testImage test = new testImage();
            byte[] prev =  await test.preview();
            byte[] im1 = await test.image1();
            byte[] im2 = await test.image2();
            model.preview = prev;
            model.images = new List<(long, byte[])>();
            model.images.Add((-1, im1));
            model.images.Add((-1, im2));
            await achievementStorage.Create(model);
        }
        // GET api/<AchievementController>/5
        [HttpGet("{id}")]
        public async Task<AchievementViewModel?> Get(long id)
        {

            return await achievementStorage.Get(id);
        }

        [HttpGet("user/{id}")]
        public async Task<List<AchievementViewModel>?> GetUsers(long id)
        {

            return await achievementStorage.GetList(id);
        }

        // PUT api/<AchievementController>/5
        [HttpPut("update")]
        public async Task<bool?> Put(AchievementBindingModel model)
        {

            testImage test = new testImage();
            byte[] prev = await test.preview();
            byte[] im1 = await test.image1();
            byte[] im2 = await test.image2();
            model.preview = im2;
            model.images = new List<(long, byte[])>();
            model.images.Add((-1, im1));
            return await achievementStorage.Update(model);
        }

        // DELETE api/<AchievementController>/5
        [HttpDelete("delete/{id}")]
        public void Delete(long id)
        {
            achievementStorage.Delete(id);
        }
    }
}
