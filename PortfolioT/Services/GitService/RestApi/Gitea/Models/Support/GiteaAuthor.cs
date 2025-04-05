using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.Services.GitService.RestApi.Gitea.Models.Support
{
    public class GiteaAuthor
    {
        public int id { get; set; }
        public string login { get; set; } = string.Empty;
    }
}
