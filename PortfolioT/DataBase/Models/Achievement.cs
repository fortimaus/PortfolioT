using PortfolioT.DataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataBase.Models
{
    public class Achievement : IAchievement
    {
        public long Id { get; set; }
        [Required]
        public string title { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string? link { get; set; } = string.Empty;

        public string? preview { get; set; } = string.Empty;

        [ForeignKey("userId")]
        public long userId { get; set; }

        public virtual List<Image> images { get; set; } = new();

        public virtual User user { get; set; } = new();
        
    }
}
