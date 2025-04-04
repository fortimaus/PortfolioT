using PortfolioT.RestApi.Models.Common;

namespace PortfolioT.RestApi.Models
{
    public interface IRepository<T,K, G>
        where T : ICommit<G>
        where K : IPullRequest
        where G : IGitFile
    {
        public long id { get; }

        public string name { get; }

        public string full_name { get; }
        public bool teamwork { get; set; }

        public string updated_at { get; }

        public string description { get; }

        public string readme { get; set; }

        public string language { get; }

        public string link { get; }

        public bool fork { get; }

        public bool empty { get; }

        public string defaultBranch { get; }

        public List<T> commits { get; }

        public List<K> pullRequests { get; }

        public int points_for_decor { get; set; }

        public int points_for_code { get; set; }

        public int points_for_teamwork { get; set; }

    }
}
