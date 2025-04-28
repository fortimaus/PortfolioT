using PortfolioT.DataContracts.BindingModels;

namespace PortfolioT.Services.LibService.Models
{
    public class Article
    {
        public string title { get; set; } = string.Empty;

        public string authors { get; set; } = string.Empty;

        public string link { get; set; } = string.Empty;

        public string desc { get; set; } = string.Empty;

        public string words { get; set; } = string.Empty;

        public int scope { get; set; } = 100;

        public ArticleBindingModel GetArticleBindingModel(long serviceId)
        {
            return new ArticleBindingModel()
            {
                title = title,
                description = desc,
                words = words,
                link = link,
                serviceId = serviceId
            };
        }
    }
}
