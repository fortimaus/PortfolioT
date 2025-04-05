using PortfolioT.Services.GitService.Models;
using PortfolioT.Services.GitService.RestApi.Gitea.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.Services.GitService.RestApi.Gitea.Models
{
    public class GiteaRepository : IRepository
    {
        public long id { get; set; }
        public string name { get; set; } = string.Empty;
        public string full_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;

        public string readme { get; set; } = string.Empty;

        public bool teamwork { get; set; } = false;
        public bool empty { get; set; }
        public bool fork { get; set; }
        public string default_branch { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;
        public string link { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;
        public List<GiteaCommit> commits { get; set; } = new List<GiteaCommit>();

        public List<GiteaPullRequest> pullRequests { get; set; } = new List<GiteaPullRequest>();

        public string defaultBranch { get => default_branch; }

        public IEnumerable<ICommit> list_commits => commits;

        public IEnumerable<IPullRequest> list_pullRequests => pullRequests;

        public string zip_path { get; set; } = string.Empty;
    }
}
