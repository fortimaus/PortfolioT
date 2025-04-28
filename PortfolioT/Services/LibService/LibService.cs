using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.Services.LibService.Models;
using PortfolioT.Services.LibService.Parsers;
using PortfolioT.DataModels.Enums;
using System.Collections.Generic;
using PortfolioT.DataModels.Models;
using PortfolioT.DataBase;

namespace PortfolioT.Services.LibService
{
    public class LibService : IService<ArticleBindingModel>
    {
        private ElibUlstuParser UlstuParser;
        public TypeService type { get; set; } = TypeService.Article;

        public LibService()
        {
            UlstuParser = new ElibUlstuParser();
        }
        public async Task<List<ArticleBindingModel>> GetUserWorks(IEnumerable<IUserService> datas)
        {
            List<Task<List<ArticleBindingModel>>> tasks = new List<Task<List<ArticleBindingModel>>>();
            foreach (var data in datas)
            {
                tasks.Add(GetArticlesByService(data));
            }
            List<ArticleBindingModel>[] list_results = await Task.WhenAll(tasks);
            List<ArticleBindingModel> res = new List<ArticleBindingModel>();
            foreach (var list in list_results)
            {
                res.AddRange(list);
            }
            return res;
        }
        public async Task<List<ArticleBindingModel>> GetArticlesByService(IUserService data)
        {
            List<Models.Article> res = new List<Models.Article>();
            if(data.serviceId == InitServices.ElibUlstu.Id)
            {
                res = await UlstuParser.getArticles(data.data);
            }
            return res.Select(x => x.GetArticleBindingModel(data.serviceId)).ToList();
        }
    }
}
