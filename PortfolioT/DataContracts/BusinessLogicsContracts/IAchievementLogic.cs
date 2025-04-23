using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.BusinessLogicsContracts
{
    public interface IAchievementStorage
    {
        List<AchievementViewModel> GetList();
        AchievementViewModel Get(long id);
        bool Create(AchievementBindingModel model);
        bool Update(AchievementBindingModel model);
        bool Delete(AchievementBindingModel model);
    }
}
