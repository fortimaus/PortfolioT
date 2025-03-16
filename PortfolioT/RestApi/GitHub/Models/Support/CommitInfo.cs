using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.GitHub.Models.Support
{
    public class CommitInfo
    {
        public Author author { get; set; } = new Author();

        public string message { get; set; } = string.Empty;
    }
}
