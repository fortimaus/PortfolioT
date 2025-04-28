using Microsoft.EntityFrameworkCore;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;

namespace PortfolioT.DataBase.Storage
{
    public class ServiceStorage : IServiceStorage
    {
        public List<ServiceViewModel> getList()
        {
            using var context = new DataBaseConnection();
            return context.Services
                .Select(x => x.GetViewModel()).ToList();
        }

        public List<ServiceViewModel> getList(TypeService type)
        {
            using var context = new DataBaseConnection();
            return context.Services
                .Where(x => x.type == type)
                .Select(x => x.GetViewModel()).ToList();
        }

    }
}
