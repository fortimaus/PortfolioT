using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.StorageContracts
{
    public interface IServiceStorage
    {
        List<ServiceViewModel> getList();
    }
}
