namespace PortfolioT.Analysis.Models
{
    public class ResponseRepository
    {
        public string title { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string link { get; set; } = string.Empty;

        public string date_create { get; set; } = string.Empty;

        public string date_last_update { get; set; } = string.Empty;

        public int score { get; set; }

        public double average_date_commit { get; set; }

        public int count_additions { get; set; }
        public int count_modifieds {get; set;}

        public int count_deletions { get; set; }

        public ResponseRepository(
            string title, string description, string date_create, 
            string date_last_update, double average_date_commit, int count_additions, 
            int count_modifieds, int count_deletions)
        {
            this.title = title;
            this.description = description;
            this.date_create = date_create;
            this.date_last_update = date_last_update;
            this.average_date_commit = average_date_commit;
            this.count_additions = count_additions;
            this.count_modifieds = count_modifieds;
            this.count_deletions = count_deletions;
        }
    }
}
