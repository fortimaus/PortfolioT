using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.Gitea.Models.Support
{
    public class Author
    {
        public int id { get; set; }
        public string login { get; set; } = string.Empty;
    }
}
