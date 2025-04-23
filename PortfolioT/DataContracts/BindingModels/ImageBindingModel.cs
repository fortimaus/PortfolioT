using PortfolioT.DataModels.Models;

namespace PortfolioT.DataContracts.BindingModels
{
    public class ImageBindingModel : IImage
    {
        public string path { get; set; } = string.Empty;

        public long Id { get; set; }
    }
}
