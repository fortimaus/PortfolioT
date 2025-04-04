namespace PortfolioT.Analysis.Models
{
    public class ResponseRepository
    {
        public string title { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string link { get; set; } = string.Empty;

        public string language { get; set; } = string.Empty;

        public float scope_for_decor { get; set; }  = 0f;
        public float scope_for_code { get; set; } = 0f;
        public float scope_bonus { get; set; } = 0f;

        public ResponseRepository(
            string title, string description, string link, string language,
            float scope_decor, float scope_code, float scope_bonus)
        {
            this.title = title;
            this.description = description;
            this.link = link;
            this.language = language;
            this.scope_for_decor = MathF.Round(scope_decor, 2);
            this.scope_for_code = MathF.Round(scope_code, 2);
            this.scope_bonus = MathF.Round(scope_bonus, 2);
        }
    }
}
