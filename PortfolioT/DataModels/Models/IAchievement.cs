namespace PortfolioT.DataModels.Models
{
    public interface IAchievement : IId
    {
        string title { get; }
        string description { get; }
        string? link { get; }
    }
}
