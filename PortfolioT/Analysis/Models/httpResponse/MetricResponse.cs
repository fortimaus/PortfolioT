namespace PortfolioT.Analysis.Models.httpResponse
{
    public class MetricResponse
    {
        public Component component { get; set; }

        public List<Measure> measures => component.measures;
    }
}
