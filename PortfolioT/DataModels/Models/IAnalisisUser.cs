namespace PortfolioT.DataModels.Models
{
    public interface IAnalisisUser
    {
        long id { get; }
        string name { get; }
        string link { get; }
        long serviceId { get; } 
    }
}
