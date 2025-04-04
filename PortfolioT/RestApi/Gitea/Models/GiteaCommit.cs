using PortfolioT.RestApi.Gitea.Models.Support;
using PortfolioT.RestApi.Models;
using PortfolioT.RestApi.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.Gitea.Models
{
    public class GiteaCommit : ICommit<GiteaCommitFile>
    {
        public string sha { get; set; }  = string.Empty;
        public GiteaCommitInfo commit { get; set; }  = new GiteaCommitInfo();

        public GiteaAuthor? author { get; set; }

        public List<GiteaCommitFile> files { get; set; }  = new List<GiteaCommitFile>();

        public GiteaStats stats { get; set; }  = new GiteaStats();


        public string commitAuthor
        { get
            {
                if (author == null)
                    return commit.author.name;
                else
                    return author.login;
            } 
        }

        public int additions { get => stats.additions; }
        public int deletions { get => stats.deletions; }

        public string message { get => commit.message; }
    }
}
