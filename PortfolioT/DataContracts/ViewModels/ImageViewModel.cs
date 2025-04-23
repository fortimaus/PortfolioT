using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.ViewModels
{
    public class ImageViewModel : IImage
    {
        public string path { get; set; } = string.Empty;

        public long Id { get; set; }
    }
}
