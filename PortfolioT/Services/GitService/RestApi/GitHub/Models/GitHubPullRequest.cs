using PortfolioT.Services.GitService.Models;
using PortfolioT.Services.GitService.RestApi.GitHub.Models.Support;

namespace PortfolioT.Services.GitService.RestApi.GitHub.Models
{
    public class GitHubPullRequest : IPullRequest
    {
        public GitHubPR_Base Base { get; set; }

        public string merged_at { get; set; } = string.Empty;
        public bool merged {
            get
            {
                if (merged_at.Length == 0)
                    return false;
                else
                    return true;
            }
        }

        public string repoFullName => Base.repo.full_name;
    }
}
