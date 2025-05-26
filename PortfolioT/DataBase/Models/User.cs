using PortfolioT.DataContracts.ViewModels;
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

        public string? code { get; set; } = string.Empty;

        public float rating { get; set; } = 0;
        public string? preview { get; set; } = string.Empty;
        [Required]
        public UserRole role { get; set; } = UserRole.None;
        [Required]
        public UserStatus status { get; set; } = UserStatus.None;

        public virtual List<UserService> services { get; set; } = new();

        [InverseProperty("moderator")]
        public virtual List<UserComment> moderatorComments { get; set; } = new();

        [InverseProperty("user")]
        public virtual List<UserComment> userComments { get; set; } = new();
        public virtual List<Achievement> achievements { get; set; } = new();

        public async Task<UserViewModel> GetViewModel()
        {
            byte[]? preview_data = null;
            if (preview != null)
                preview_data = await File.ReadAllBytesAsync(preview);
            return new UserViewModel
            {
                Id = Id,
                login = login,
                password = password,
                email = email,
                about = about,
                role = role,
                status = status,
                preview = preview_data,
                rating = rating
            };
        }
    }
}
