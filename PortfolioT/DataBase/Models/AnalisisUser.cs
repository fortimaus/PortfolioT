using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataBase.Models
{
    public class AnalisisUser
    {
        public long Id { get; set; }

        [Required]
        public string name { get; set; } = string.Empty;
        [Required]
        public string link { get; set; } = string.Empty;

        [ForeignKey("serviceId")]
        public long serviceId { get; set; }
        public virtual Service service { get; set; } = new();
    }
}
