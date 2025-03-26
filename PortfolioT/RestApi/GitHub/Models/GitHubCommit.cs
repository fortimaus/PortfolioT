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

        public string commitAuthor { get => commit.author.name; }

        public string date { get => commit.author.date; }

        public List<GitHubCommitFile> files { get; set; }

        public int additions { get => stats.additions; }

        public int deletions { get => stats.deletions; }
    }
}
