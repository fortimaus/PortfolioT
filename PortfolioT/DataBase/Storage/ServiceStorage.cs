using Microsoft.EntityFrameworkCore;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;

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

        
    }
}
