using PortfolioT.DataBase.Models;
using PortfolioT.DataModels.Enums;

namespace PortfolioT.DataBase
{
    public static class InitServices
    {
        public static Service GitHub { get; } = new Service()
        {
            Id = 1,
            title = "GitHub",
            type = TypeService.Repository
        };
        public static Service GitUlstu { get; } = new Service()
        {
            Id = 2,
            title = "GitUlstu",
            type = TypeService.Repository
        };
        public static Service ElibUlstu { get; } = new Service()
        {
            Id = 3,
            title = "ElibUlstu",
            type = TypeService.Article
        };
    }
}
