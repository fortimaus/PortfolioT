using PortfolioT.DataBase.Commons;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.StorageContracts
{
    public interface IAnalisisRepoStorage
    {
        bool Create(AnalisisRepoBindingModel model);

        bool CreateList(List<AnalisisRepoBindingModel> models);

        bool Delete(long id);
        bool DeleteMany(List<long> ids);
        AnalisisRepoViewModel GetOne(long userId, string repo);
        CompareUserRepoInfo GetAverage( long userId);
    }
}
