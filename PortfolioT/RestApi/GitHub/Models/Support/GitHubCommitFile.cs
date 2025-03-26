using PortfolioT.RestApi.Models.Common;
using PortfolioT.RestApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.GitHub.Models.Support
{
    public class GitHubCommitFile : IGitFile
    {
        public string fileName { get; set; } = string.Empty;

        public string status { get; set; } = string.Empty;

        public GitFileStatus fileStatus { 
            get
            {
                switch (status)
                {
                    case "added":
                        return GitFileStatus.ADD;
                    case "modified":
                        return GitFileStatus.MODIFIED;
                    case "removed":
                        return GitFileStatus.REMOVE;
                    default:
                        return GitFileStatus.NONE;
                }
            } 
        }

        public int additions { get; set; }

        public int deletions { get; set; }
    }
}
