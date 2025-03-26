using PortfolioT.RestApi.Gitea.Models.Support;
using PortfolioT.RestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.Gitea.Models
{
    public class GiteaRepository : IRepository<GiteaCommit, GiteaPullRequest, GiteaCommitFile>
    {
        public long id { get; set; }
        public string name { get; set; } = string.Empty;
        public string full_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public bool empty { get; set; }
        public bool fork { get; set; }
        public int stars_count { get; set; } = 0;
        public int forks_count { get; set; } = 0;
        public string default_branch { get; set; } = string.Empty;
        public string created_at { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        public string link { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;


        public List<GiteaCommit> commits { get; set; } = new List<GiteaCommit>();

        public List<GiteaPullRequest> pullRequests { get; set; } = new List<GiteaPullRequest>();

        public string defaultBranch { get => default_branch; }

    }
}
