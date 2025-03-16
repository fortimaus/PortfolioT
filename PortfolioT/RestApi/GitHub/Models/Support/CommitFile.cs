using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.GitHub.Models.Support
{
    public class CommitFile
    {
        public string filename { get; set; } = string.Empty;

        public string status { get; set; } = string.Empty;

        public int additions { get; set; }

        public int deletions { get; set; }
    }
}
