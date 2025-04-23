using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataBase.Models
{
    public class Article : Achievement
    {
        public string? words { get; set; } = string.Empty;
    }
}
