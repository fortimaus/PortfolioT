using PortfolioT.DataContracts.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataBase.Models
{
    public class Article : Achievement
    {
        public string? words { get; set; } = string.Empty;

        new public async Task<ArticleViewModel> GetViewModel()
        {
            byte[]? preview_data = null;
            if (preview != null)
                preview_data = await File.ReadAllBytesAsync(preview);

            Dictionary<long, byte[]> ims = new Dictionary<long, byte[]>();
            foreach (var image in images)
                ims.Add(image.Id, await File.ReadAllBytesAsync(image.path));
            return new ArticleViewModel()
            {
                Id = Id,
                title = title,
                description = description,
                link = link,
                preview = preview_data,
                words = words,
                images = ims
            };
        }
    }
}
