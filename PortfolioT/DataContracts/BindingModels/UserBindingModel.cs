using PortfolioT.DataModels.Enums;
using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.BindingModels
{
    public class UserBindingModel : IUser
    {
        public long Id { get; set; }
        public string login { get; set; } = string.Empty;

        public string password { get; set; } = string.Empty;

        public string email { get; set; } = string.Empty;

        public string? about { get; set; } = string.Empty;

        public byte[]? preview { get; set; }

        public string code { get; set; } = string.Empty;

        public UserRole role { get; set; } = UserRole.None;

        public UserStatus status { get; set; } = UserStatus.None;

    


    }
}
