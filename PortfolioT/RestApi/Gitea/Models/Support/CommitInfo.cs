using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.Gitea.Models.Support
{
    public class CommitInfo
    {
        public string message { get; set; } = string.Empty;

        public CommitAuthor author { get; set; } = new CommitAuthor();

    }
}
