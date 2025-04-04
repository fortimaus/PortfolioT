using PortfolioT.RestApi.GitHub.Models.Support;
using PortfolioT.RestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.GitHub.Models
{
    public class GitHubRepository : IRepository<GitHubCommit, GitHubPullRequest, GitHubCommitFile>
    {
        public long id { get; set; }

        public string name { get; set; } = string.Empty;

        public string full_name { get; set; } = string.Empty;

        public bool teamwork { get; set; } = false;

        public string updated_at { get; set; } = string.Empty;

        public bool fork { get; set; } = false;


        public int size { get; set; }


        public string description { get; set; } = string.Empty;

        public string readme { get; set; } = string.Empty;

        public string default_branch { get; set; } = string.Empty;

        public string language { get; set; } = string.Empty;

        public string link { get; set; } = string.Empty;

        public List<GitHubCommit> commits { get; set; } = new List<GitHubCommit>();

        public List<GitHubPullRequest> pullRequests { get; set; } = new List<GitHubPullRequest>();

        public bool empty { 
            get
            {
                if (size == 0)
                    return true;
                else
                    return false;
            } 
        }

        public string defaultBranch => default_branch;

        public int points_for_decor { get; set; }

        public int points_for_code { get; set; }

        public int points_for_teamwork { get; set; }

    }
}
