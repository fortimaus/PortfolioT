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

            Dictionary<long, byte[]> ims = new Dictionary<long, byte[]>();
            foreach (var image in images)
                ims.Add(image.Id, await File.ReadAllBytesAsync(image.path));
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
