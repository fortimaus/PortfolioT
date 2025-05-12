using PortfolioT.Controllers.Commons;
using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.ViewModels
{
    public class AchievementViewModel : IAchievement
    {
        public string title { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string? link { get; set; } = string.Empty;

        public byte[]? preview { get; set; }

        public List<ImageResponse>? images { get; set; } = new();

        public long Id { get; set; }

        public long serviceId { get; set; }
    }
}
