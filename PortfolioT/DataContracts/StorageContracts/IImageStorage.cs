using PortfolioT.DataBase;
using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.StorageContracts
{
    public interface IImageStorage
    {
        Task<bool> Create(DataBaseConnection context, List<byte[]> images, string path, string name, long id);
        bool Delete(DataBaseConnection context, List<Image> images);
    }
}
