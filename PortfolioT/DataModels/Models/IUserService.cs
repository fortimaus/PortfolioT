using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataModels.Models
{
    public interface IUserService
    {
        long userId { get; set; }
        long serviceId { get; set; }
        string data { get; }
    }
}
