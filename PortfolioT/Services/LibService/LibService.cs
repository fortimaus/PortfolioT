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

            return await UlstuParser.getArticles("Романов", "2020", "2025");
        }
    }
}
