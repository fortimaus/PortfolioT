using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.ViewModels
{
    public class ServiceViewModel : IService
    {
        public string title { get; set; } = string.Empty;

        public long Id { get; set; }
    }
}
