using PortfolioT.DataContracts.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataBase.Models
{
    public class AnalisysRepo
    {
        public long Id { get; set; }
        [Required]
        public string title { get; set; } = string.Empty;

        public string? link { get; set; } = string.Empty;
        public float scope_decor { get; set; } = 0;
        [Required]
        public float scope_code { get; set; } = 0;

        [Required]
        public float scope_security { get; set; } = -1;
        [Required]
        public float scope_maintability { get; set; } = -1;
        [Required]
        public float scope_reability { get; set; } = -1;

        [ForeignKey("userId")]
        public long userId { get; set; }
        public virtual AnalisisUser user { get; set; } = new();

        public DateTime date { get; set; }

        public AnalisisRepoViewModel GetAnalisisRepoViewModel()
        {
            return new AnalisisRepoViewModel()
            {
                id = Id,
                title = title,
                userName = user.name,
                userLink = user.link,
                link = link,
                scope_decor = scope_code,
                scope_code = scope_code,
                scope_maintability = scope_maintability,
                scope_reability = scope_reability,
                scope_security = scope_security,
                date = date
            };
        }
    }
}
