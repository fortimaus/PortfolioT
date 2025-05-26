using PortfolioT.Controllers.Commons;

namespace PortfolioT.DataBase.Commons
{
    public class CompareUserRepoInfo
    {
        public Dictionary<string, int> languageCounts { get; set; } = new Dictionary<string, int>();
        public float scopeSecurity { get; set; } = 0;
        public float scopeMaintability { get; set; } = 0;
        public float scopeReability { get; set; } = 0;
        public float scopeDecor { get; set; } = 0;
        public float scopeCode { get; set; } = 0;
        public float scopeTeam { get; set; } = 0;
    }
}
