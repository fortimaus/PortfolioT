namespace PortfolioT.DataModels.Models
{
    public interface IRepo : IAchievement
    {
        string? language { get; }
        float scope_decor { get; }
        float scope_code { get; }
        float scope_team { get; }
        float scope_security { get; }
        float scope_maintability { get; }
        float scope_reability { get; }
    }
}
