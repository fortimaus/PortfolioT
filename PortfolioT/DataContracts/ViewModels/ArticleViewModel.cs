using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.ViewModels
{
    public class ArticleViewModel : IArticle
    {
        public string? words { get; set; } = string.Empty;

        public string title { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string? link { get; set; } = string.Empty;

        public byte[]? preview { get; set; }

        public Dictionary<long, byte[]>? images { get; set; } = new();

        public long Id { get; set; }

        public long serviceId { get; set; }
    }
}
