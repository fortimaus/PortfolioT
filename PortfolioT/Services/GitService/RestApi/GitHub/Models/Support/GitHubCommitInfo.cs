using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.Services.GitService.RestApi.GitHub.Models.Support
{
    public class GitHubCommitInfo
    {
        public GitHubAuthor author { get; set; } = new GitHubAuthor();

        public string message { get; set; } = string.Empty;
    }
}
