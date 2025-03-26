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

        public string created_at { get; set; } = string.Empty;

        public string updated_at { get; set; } = string.Empty;

        public bool fork { get; set; } = false;

        public int stargazers_count { get; set; } = 0;

        public int forks_count { get; set; } = 0;

        public int size { get; set; }


        public string description { get; set; } = string.Empty;

        public string default_branch { get; set; } = string.Empty;

        public string language { get; set; } = string.Empty;

        public string link { get; set; } = string.Empty;

        public List<GitHubCommit> commits { get; set; } = new List<GitHubCommit>();

        public List<GitHubPullRequest> pullRequests { get; set; } = new List<GitHubPullRequest>();

        public int stars_count => stargazers_count;

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

    }
}
