using PortfolioT.RestApi.GitHub.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.GitHub.Models
{
    public class Commit
    {
        public string sha { get; set; } = string.Empty;

        public CommitInfo commit { get; set; } = new CommitInfo();

        public Stats stats { get; set; } = new Stats();

        public List<CommitFile> files { get; set; } = new List<CommitFile>();
    }
}
