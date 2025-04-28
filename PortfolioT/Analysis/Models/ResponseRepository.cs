using PortfolioT.DataContracts.BindingModels;
using PortfolioT.Services.GitService.Models;

namespace PortfolioT.Analysis.Models
{
    public class ResponseRepository
    {
        public string title { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string link { get; set; } = string.Empty;

        public string language { get; set; } = string.Empty;

        public float scope_for_decor { get; set; }  = 0;
        public float scope_for_code { get; set; } = 0;
        public float scope_bonus { get; set; } = 0;

        DateTime date { get; set; }

        public float scope_security { get; set; } = 1;
        public float scope_maintability { get; set; } = 1;
        public float scope_reability { get; set; } = 1;

        public string comments { get; set; } = string.Empty;
        public ResponseRepository(
            string title, string description, string link, string language,
            float scope_for_decor, float scope_for_code, float scope_bonus,
            float scope_security, float scope_maintability, float scope_reability,
            DateTime date,string comments)
        {
            this.title = title;
            this.description = description;
            this.link = link;
            this.language = language;
            this.scope_security = scope_security;
            this.scope_maintability = scope_maintability;
            this.scope_reability = scope_reability;
            this.scope_for_decor = MathF.Round(scope_for_decor, 2);
            this.scope_for_code = MathF.Round(scope_for_code, 2);
            this.scope_bonus = MathF.Round(scope_bonus, 2);
            this.date = date;
            this.comments = comments;
        }
        public RepoBindingModel GetRepoBindingModel(long serviceId)
        {
            return new RepoBindingModel()
            {
                title = title,
                description = description,
                link = link,
                language = language,
                scope_code = scope_for_code,
                scope_decor = scope_for_decor,
                scope_team = scope_bonus,
                scope_maintability = scope_maintability,
                scope_reability = scope_reability,
                scope_security = scope_security,
                serviceId = serviceId,
                comments = comments,
                date = DateOnly.FromDateTime(date).ToString()
            };
        }
       
    }
}
