using PortfolioT.Models.Request;

namespace PortfolioT.Services
{
    public interface IService<T>
    {
        Task<List<T>> GetUserWorks(List<ServiceData> datas);
    }
}
