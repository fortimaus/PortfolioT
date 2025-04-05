namespace PortfolioT.Services.GitService.Models
{
    public interface ICommit
    {
        public string sha { get; }

        public string commitAuthor { get; }

        public string message { get; }

        public IEnumerable<IGitFile> list_files{ get; }

        public int additions { get; }

        public int deletions { get; }
    }
}
