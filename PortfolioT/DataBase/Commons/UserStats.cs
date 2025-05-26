namespace PortfolioT.DataBase.Commons
{
    public class UserStats
    {
        public int countRepos { get; set; } = 0;
        public int countArticles { get; set; } = 0;
        public int countAchievements { get; set; } = 0;

        public float rating { get; set; } = 0;

        public float averageScopeRepos { get; set; } = 0;
        public float scopeArticles { get; set; } = 0;

        public CompareUserRepoInfo repoInfo { get; set; } = new CompareUserRepoInfo();

    }
}
