using PortfolioT.Services.LibService.Models;

namespace PortfolioT.Services.LibService
{
    public interface IParser
    {
        Task<List<Article>> getArticles(string info);
    }
}
