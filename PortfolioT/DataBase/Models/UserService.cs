using PortfolioT.DataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataBase.Models
{
    public class UserService : IUserService
    {
        [ForeignKey("userId")]
        public long userId { get; set; }
        [ForeignKey("serviceId")]
        public long serviceId { get; set; }

        [Required]
        public string data { get; set; } = string.Empty;

        public virtual User user { get; set; } = new();

        public virtual Service service { get; set; } = new();
        
    }
}
