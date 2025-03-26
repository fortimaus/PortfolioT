using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.Gitea.Models.Support
{
    public class GiteaCommitInfo
    {
        public string message { get; set; } = string.Empty;

        public GiteaCommitAuthor author { get; set; } = new GiteaCommitAuthor();

    }
}
