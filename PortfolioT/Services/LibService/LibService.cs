using PortfolioT.Models.Request;
using PortfolioT.Services.LibService.Models;

namespace PortfolioT.Services.LibService
{
    public class LibService : IService<Article>
    {
        private ElibUlstuParser UlstuParser;
        public LibService()
        {
            UlstuParser = new ElibUlstuParser();
        }
        public async Task<List<Article>> GetUserWorks(List<ServiceData> datas)
        {
            string[] lines = datas[0].data.Split('-');
            return await UlstuParser.getArticles(lines[0], lines[1], lines[2]);
        }
    }
}
