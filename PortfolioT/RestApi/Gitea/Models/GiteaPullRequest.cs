using PortfolioT.RestApi.Gitea.Models.Support;
using PortfolioT.RestApi.Models;

namespace PortfolioT.RestApi.Gitea.Models
{
    public class GiteaPullRequest : IPullRequest
    {
        public bool merged { get; set; }  = false;

        public GiteaPR_Base Base;
        
        public string repoFullName { get => Base.repo.full_name; }


    }
}
