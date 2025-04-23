using PortfolioT.DataModels.Enums;

namespace PortfolioT.DataModels.Models
{
    public interface IUser : IId
    {
        string login { get; }
        string password { get; }
        string email { get; }
        string? about { get; }
        string? preview { get; }
        
        UserRole role { get; }
        UserStatus status { get; }


    }
}
