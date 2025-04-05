using PortfolioT.Services.GitService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.Services.GitService.RestApi.Gitea.Models.Support
{
    public class GiteaCommitFile : IGitFile
    {
        public string sha { get; set; } = string.Empty;

       
    }
}
