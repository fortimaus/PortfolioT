using PortfolioT.DataModels.Enums;

namespace PortfolioT.DataModels.Models
{
    public interface IService : IId
    {
        string title { get; }

        TypeService type { get; set; }
    }
}
