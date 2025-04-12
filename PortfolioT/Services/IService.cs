using PortfolioT.Models.Request;

namespace PortfolioT.Services
{
    public interface IService<T>
    {
        public Task<List<T>> GetUserWorks(List<ServiceData> datas);
    }
}
