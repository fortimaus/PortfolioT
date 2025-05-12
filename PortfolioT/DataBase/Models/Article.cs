using PortfolioT.Controllers.Commons;
using PortfolioT.DataContracts.ViewModels;
using Swashbuckle.AspNetCore.SwaggerGen;
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

            List<ImageResponse> ims = new List<ImageResponse>();
            foreach (var image in images)
                ims.Add(new ImageResponse()
                {
                    id = image.Id, 
                    image = await File.ReadAllBytesAsync(image.path)
                });
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
