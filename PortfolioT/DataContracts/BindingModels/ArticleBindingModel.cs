using PortfolioT.Controllers.Commons;
using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.BindingModels
{
    public class ArticleBindingModel : IArticle
    {
        public string? words { get; set; } = string.Empty;

        public string title { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string? link { get; set; } = string.Empty;

        public long userId { get; set; }

        public byte[]? preview { get; set; }

        public List<ImageResponse>? images { get; set; } = new();
        public long Id { get; set; }

        public long serviceId { get; set; }
    }
}
