namespace PortfolioT.DataModels.Models
{
    public interface IAnalisisRepo
    {
        string title { get; }
        string? link { get; }
        float scope_decor { get; }
        float scope_code { get; }
        float scope_security { get; }
        float scope_maintability { get; }
        float scope_reability { get; }

        long userId { get; }

    }
}
