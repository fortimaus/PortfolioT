namespace PortfolioT.Services.GitService.Models
{
    public interface IRepository
    {
        public long id { get; }

        public string name { get; }

        public string full_name { get; }
        public bool teamwork { get; set; }

        public string updated_at { get; }

        public string zip_path { get; }
        public string dir_path { get; set; }

        public string description { get; }

        public string readme { get; set; }

        public string language { get; }

        public string link { get; }

        public bool fork { get; }

        public bool empty { get; }

        public string defaultBranch { get; }

        public IEnumerable<ICommit> list_commits { get; }

        public IEnumerable<IPullRequest> list_pullRequests { get; }

        public float scope_decor { get; set; }
        public float scope_code { get; set; }
        public float scope_bonus { get; set; }

    }
}
