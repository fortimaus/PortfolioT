using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.BindingModels
{
    public class UserServiceBindingModel : IUserService
    {
        public long userId { get; set; }
        public long serviceId { get; set; }
        public string data { get; set; } = string.Empty;
    }
}
