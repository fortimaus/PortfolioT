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
        public string created_at { get; }

        public string updated_at { get; }

        public string description { get; }

        public string language { get; }

        public string link { get; }

        public bool fork { get; }

        public int stars_count { get; }

        public int forks_count { get; }

        public bool empty { get; }

        public string defaultBranch { get; }

        public List<T> commits { get; }

        public List<K> pullRequests { get; }
    }
}
