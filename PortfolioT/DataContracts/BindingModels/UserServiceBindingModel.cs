using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.BindingModels
{
    public class UserServiceBindingModel : IUserService
    {

        public string data { get; set; } = string.Empty;
    }
}
