namespace PortfolioT.Services.GitService.Models
{
    public interface IPullRequest
    {
        public bool merged { get; }

        public string repoFullName { get; }
    }
}
