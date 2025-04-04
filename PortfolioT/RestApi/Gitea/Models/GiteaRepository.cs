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

        public string readme { get; set; } = string.Empty;

        public bool teamwork { get; set; } = false;
        public bool empty { get; set; }
        public bool fork { get; set; }
        public string default_branch { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        public string link { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;


        public List<GiteaCommit> commits { get; set; } = new List<GiteaCommit>();

        public List<GiteaPullRequest> pullRequests { get; set; } = new List<GiteaPullRequest>();

        public string defaultBranch { get => default_branch; }

        public int points_for_decor { get; set; }

        public int points_for_code { get; set; }

        public int points_for_teamwork { get; set; }

    }
}
