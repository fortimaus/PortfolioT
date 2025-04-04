using PortfolioT.RestApi.GitHub.Models.Support;
using PortfolioT.RestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.GitHub.Models
{
    public class GitHubCommit : ICommit<GitHubCommitFile>
    {
        public string sha { get; set; } = string.Empty;

        public GitHubCommitInfo commit { get; set; } = new GitHubCommitInfo();

        public GitHubStats stats { get; set; } = new GitHubStats();

        public GitHubCommitAuthor? author { get; set; }

        public string commitAuthor { get
            {
                if (author == null)
                    return commit.author.name;
                else
                    return author.login;
            }
        }


        public List<GitHubCommitFile> files { get; set; }

        public int additions { get => stats.additions; }

        public int deletions { get => stats.deletions; }

        public string message { get => commit.message; }
    }
}
