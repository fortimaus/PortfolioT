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
        
    }
}
