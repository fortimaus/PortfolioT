using PortfolioT.Controllers.Commons;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace PortfolioT.DataBase.Models
{
    public class Achievement : IAchievement
    {
        public long Id { get; set; }
        [Required]
        public string title { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string? link { get; set; } = string.Empty;
        public bool basic { get; set; } = true;

        public string? preview { get; set; } = string.Empty;

        [ForeignKey("userId")]
        public long userId { get; set; }

        [ForeignKey("serviceId")]
        public long? serviceId { get; set; }

        public virtual List<Image> images { get; set; } = new();

        public virtual User user { get; set; } = new();

        public virtual Service? service { get; set; } = new();


        public async Task<AchievementViewModel> GetViewModel()
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
            return new AchievementViewModel()
            {
                Id = Id,
                title = title,
                description = description,
                link = link,
                preview = preview_data,
                images = ims
            };
        }

    }
}
