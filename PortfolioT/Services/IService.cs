using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataModels.Models;

namespace PortfolioT.Services
{
    public interface IService<T>
    {
        public Task<List<T>> GetUserWorks(IEnumerable<IUserService> datas);
    }
}
