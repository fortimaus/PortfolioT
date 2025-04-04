using PortfolioT.RestApi.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.Gitea.Models.Support
{
    public class GiteaCommitFile : IGitFile
    {
        public string sha { get; set; } = string.Empty;

       
    }
}
