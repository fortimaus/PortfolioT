using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.BindingModels
{
    public class ServiceBindingModel : IService
    {
        public string title { get; set; } = string.Empty;

        public long Id { get; set; }
    }
}
