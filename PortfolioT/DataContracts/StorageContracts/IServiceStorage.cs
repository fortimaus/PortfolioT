using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;

namespace PortfolioT.DataContracts.StorageContracts
{
    public interface IServiceStorage
    {
        List<ServiceViewModel> getList();

        List<ServiceViewModel> getList(TypeService type);
    }
}
