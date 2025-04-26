using PortfolioT.DataModels.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataContracts.ViewModels
{
    public class UserServiceViewModel : IUserService
    {
        public long userId { get; set; }
        public long serviceId { get; set; }
        public string serviceName { get; set; } = string.Empty;
        public string data { get; set; } = string.Empty;
    }
}
