using PortfolioT.DataModels.Enums;
using PortfolioT.DataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataBase.Models
{
    public class User : IUser
    {
        public long Id { get; set; }
        [Required]
        public string login { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
        [Required]
        public string email { get; set; } = string.Empty;

        public string? about { get; set; } = string.Empty;

        public string? preview { get; set; } = string.Empty;
        [Required]
        public UserRole role { get; set; } = UserRole.None;
        [Required]
        public UserStatus status { get; set; } = UserStatus.None;

        public virtual List<UserService> services { get; set; } = new();
        public virtual List<Achievement> achievements { get; set; } = new();


    }
}
