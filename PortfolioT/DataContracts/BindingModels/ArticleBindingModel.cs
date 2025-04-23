using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.BindingModels
{
    public class ArticleBindingModel : IArticle
    {
        public string? words { get; set; } = string.Empty;

        public string title { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string? link { get; set; } = string.Empty;

        public string? preview { get; set; } = string.Empty;


        public long Id { get; set; }
    }
}
