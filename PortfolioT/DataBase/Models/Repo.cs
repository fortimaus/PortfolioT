using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataBase.Models
{

    public class Repo : Achievement
    {
        
        public string? language { get; set; } = string.Empty;
        [Required]
        public float scope_decor { get; set; } = 0;
        [Required]
        public float scope_code { get; set; } = 0;
        [Required]
        public float scope_team { get; set; } = -1;
        [Required]
        public float scope_security { get; set; } = -1;
        [Required]
        public float scope_maintability { get; set; } = -1;
        [Required]
        public float scope_reability { get; set; } = -1;

        public DateOnly? date { get; set; }

        public string? comments { get; set; } = string.Empty;

        new public async Task<RepoViewModel> GetViewModel()
        {
            byte[]? preview_data = null;
            if (preview != null)
                preview_data = await File.ReadAllBytesAsync(preview);

            Dictionary<long, byte[]> ims = new Dictionary<long, byte[]>();
            foreach (var image in images)
                ims.Add(image.Id, await File.ReadAllBytesAsync(image.path));
            return new RepoViewModel()
            {
                Id = Id,
                title = title,
                description = description,
                link = link,
                preview = preview_data,
                language = language,
                scope_decor = scope_decor,
                scope_code = scope_code,
                scope_team = scope_team,
                scope_security = scope_security,
                scope_maintability = scope_maintability,
                scope_reability = scope_reability,
                images = ims
            };
        }
    }
}
