using PortfolioT.DataBase.Models;
using PortfolioT.DataModels.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataContracts.ViewModels
{
    public class AnalisisRepoViewModel : IAnalisisRepo
    {
        public string title { get; set; } = string.Empty;

        public long id { get; set; }
        public string? link { get; set; } = string.Empty;
        public string? userLink { get; set; } = string.Empty;
        public float scope_decor { get; set; } = 0;
        public float scope_code { get; set; } = 0;

        public float scope_security { get; set; } = -1;
        public float scope_maintability { get; set; } = -1;
        public float scope_reability { get; set; } = -1;

        public string userName { get; set; } = string.Empty;

        public Dictionary<string, int> langugeCount = new Dictionary<string, int>();
        public long userId { get; set; }
        public DateTime date {get; set;}
    }
}
