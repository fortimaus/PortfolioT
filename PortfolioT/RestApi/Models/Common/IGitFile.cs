using PortfolioT.RestApi.Models.Enums;

namespace PortfolioT.RestApi.Models.Common
{
    public interface IGitFile
    {
        public string fileName { get; }

        public GitFileStatus fileStatus { get; }
    }
}
