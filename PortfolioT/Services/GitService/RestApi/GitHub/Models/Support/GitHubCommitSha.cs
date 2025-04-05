using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.Services.GitService.RestApi.GitHub.Models.Support
{
    public class GitHubCommitSha
    {
        public string sha { get; set; } = string.Empty;

        public GitHubCommitInfo commit { get; set; } = new GitHubCommitInfo();
        public GitHubCommitAuthor? author { get; set; }

        public string CommitAuthor { get 
            {
                if (author == null)
                    return commit.author.name;
                else
                    return author.login;
            } }
    }
}
