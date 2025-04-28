using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;

namespace PortfolioT.BusinessLogic.Logics
{
    public class ServiceLogic : IServiceLogic
    {
        ServiceStorage serviceStorage;

        public ServiceLogic()
        {
            serviceStorage = new ServiceStorage();
        }
        public List<ServiceViewModel> getList()
        {
            try
            {
                return serviceStorage.getList();

            }
            catch
            {
                throw;
            }
        }

        public List<ServiceViewModel> getList(TypeService type)
        {
            try
            {
                return serviceStorage.getList(type);

            }
            catch
            {
                throw;
            }
        }
    }
}
