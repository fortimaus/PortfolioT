namespace PortfolioT.Analysis.Models
{
    public class SummaryInfo
    {
        public Dictionary<string, DateTime> dates { get; set; } = new Dictionary<string, DateTime>();

        public int additions { get; set; } = 0;

        public int deletions { get; set; } = 0;
        public int modified { get; set; } = 0;

        public void addDate(string sha, string date)
        {
            if (dates.ContainsKey(sha))
                return;
            else
                dates.Add(sha, DateTime.Parse(date));
        }

        public void addAdditions()
        {
            additions += 1;
        }
        public void addModefieds()
        {
            modified += 1;
        }
        public void addDeletions()
        {
            deletions += 1;
        }
    }
}
