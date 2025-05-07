using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.Controllers.Commons
{
    public class LoginResponse
    {
        public string token { get; set; } = string.Empty;
        public UserViewModel user { get; set; } = new();
    }
}
