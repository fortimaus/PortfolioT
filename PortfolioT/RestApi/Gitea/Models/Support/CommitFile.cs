using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.Gitea.Models.Support
{
    public class CommitFile
    {
        public string filename { get; set; } = string.Empty;

        public string status { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Filename: {filename} Status: {status}";
        }
    }
}
