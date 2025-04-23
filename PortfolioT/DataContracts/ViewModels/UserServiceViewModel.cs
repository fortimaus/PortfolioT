using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.ViewModels
{
    public class UserServiceViewModel : IUserService
    {
        public string data { get; set; } = string.Empty;
    }
}
