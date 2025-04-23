using PortfolioT.DataModels.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataBase.Models
{
    public class Image : IImage
    {
        public string path { get; set; } = string.Empty;

        public long Id { get; set; }

        [ForeignKey("achievementId")]
        public long achievementId { get; set; }

        public virtual Achievement achievement { get; set; } = new();

    }
}
