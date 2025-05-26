namespace PortfolioT.Controllers.Commons
{
    public class CompareUserServiceResponse
    {
        public Dictionary<string, int> myLanguageCounts { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> compareLanguageCounts { get; set; } = new Dictionary<string, int>();

        public float mySecurity { get; set; } = 0;
        public float myMaintability { get; set; } = 0;
        public float myReability { get; set; } = 0;
        public float myDecor { get; set; } = 0;
        public float myCode { get; set; } = 0;

        public float compareSecurity { get; set; } = 0;
        public float compareMaintability { get; set; } = 0;
        public float compareReability { get; set; } = 0;
        public float compareDecor { get; set; } = 0;
        public float compareCode { get; set; } = 0;

    }
}
