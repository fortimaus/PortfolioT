using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;

namespace PortfolioT.DataContracts.BusinessLogicsContracts
{
    public interface IServiceLogic
    {
        List<ServiceViewModel> getList();

        List<ServiceViewModel> getList(TypeService type);
    }
}
