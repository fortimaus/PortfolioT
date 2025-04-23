using PortfolioT.DataModels.Models;
using System.ComponentModel.DataAnnotations;

namespace PortfolioT.DataBase.Models
{
    public class Service : IService
    {
        [Required]
        public string title { get; set; } = string.Empty;

        public long Id { get; set; }

        public virtual List<UserService> userServices { get; set; } = new();
    }
}
