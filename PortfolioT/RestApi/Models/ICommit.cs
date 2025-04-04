using PortfolioT.RestApi.Models.Common;

namespace PortfolioT.RestApi.Models
{
    public interface ICommit<T>
        where T: IGitFile
    {
        public string sha { get; }

        public string commitAuthor { get; }

        public string message { get; }

        public List<T> files{ get; }

        public int additions { get; }

        public int deletions { get; }
    }
}
