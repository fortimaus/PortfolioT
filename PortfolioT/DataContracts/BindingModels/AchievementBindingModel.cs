using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.BindingModels
{
    public class AchievementBindingModel : IAchievement
    {
        public string title { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public long userId { get; set; }

        public string? link { get; set; } = string.Empty;

        public byte[]? preview { get; set; }

        public List<(long, byte[])>? images { get; set; } = new();

        public long Id { get; set; }

        public long serviceId { get; set; }
    }
}
