using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.BindingModels
{
    public class RepoBindingModel : IRepo
    {
        public string? language { get; set; } = string.Empty;

        public float scope_decor { get; set; }

        public float scope_code { get; set; }

        public float scope_team { get; set; }

        public float scope_security { get; set; }

        public float scope_maintability { get; set; }

        public float scope_reability { get; set; }

        public string title { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string? link { get; set; } = string.Empty;

        public long userId { get; set; }

        public byte[]? preview { get; set; }

        public string comments { get; set; } = string.Empty;
        public string date { get; set; } = string.Empty;

        public List<(long, byte[])>? images { get; set; } = new();

        public long Id { get; set; }

        public long serviceId { get; set; }
    }
}
