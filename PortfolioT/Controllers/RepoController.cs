using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.Controllers
{
    [Route("api/reposotories")]
    [ApiController]
    public class RepoController : ControllerBase
    {
        RepoStorage repoStorage = new RepoStorage();
        [HttpPost("create")]
        public async void Post(RepoBindingModel model)
        {
            testImage test = new testImage();
            byte[] prev = await test.preview();
            byte[] im1 = await test.image1();
            byte[] im2 = await test.image2();
            model.preview = prev;
            model.images = new List<(long, byte[])>();
            model.images.Add((-1, im1));
            model.images.Add((-1, im2));
            await repoStorage.Create(model);
        }
        [HttpGet("{id}")]
        public async Task<RepoViewModel?> Get(long id)
        {

            return await repoStorage.Get(id);
        }

        [HttpGet("user/{id}")]
        public async Task<List<RepoViewModel>?> GetUsers(long id)
        {

            return await repoStorage.GetList(id);
        }

        [HttpPut("update")]
        public async Task<bool?> Put(RepoBindingModel model)
        {

            testImage test = new testImage();
            byte[] prev = await test.preview();
            byte[] im1 = await test.image1();
            byte[] im2 = await test.image2();
            model.preview = im2;
            model.images = new List<(long, byte[])>();
            model.images.Add((-1, im1));
            return await repoStorage.Update(model);
        }

        [HttpDelete("delete/{id}")]
        public void Delete(long id)
        {
            repoStorage.Delete(id);
        }
    }
}
