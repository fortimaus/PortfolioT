using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataModels.Models
{
    public interface IUserComment : IId
    {
        string text { get; set; } 

        long moderatorId { get; set; }

        long userId { get; set; }

        public DateTime date { get; set; }
    }
}
