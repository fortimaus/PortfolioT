using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.Gitea.Models
{
    public class Branch
    {
        public string name { get; set; } = string.Empty;

        public List<Commit> commits { get; set; } = new List<Commit>();

    }
}
