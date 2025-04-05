using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.Services.GitService.RestApi.Gitea.Models.Support
{
    public class GiteaCommitAuthor
    {
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
    }
}
