using PortfolioT.Services.GitService.Models;
using PortfolioT.Services.GitService.RestApi.Gitea.Models.Support;

namespace PortfolioT.Services.GitService.RestApi.Gitea.Models
{
    public class GiteaPullRequest : IPullRequest
    {
        public bool merged { get; set; }  = false;

        public GiteaPR_Base Base;
        
        public string repoFullName { get => Base.repo.full_name; }


    }
}
