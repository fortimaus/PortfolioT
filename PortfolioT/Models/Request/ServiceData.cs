using PortfolioT.Models.Enums;

namespace PortfolioT.Models.Request
{
    public class ServiceData
    {
        public string name_service { get; set; } = string.Empty;

        public string data { get; set; } = string.Empty;

        public TypeServiceData type { get; set; }
    }
}
