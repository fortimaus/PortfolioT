using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.BusinessLogicsContracts
{
    public interface IAchievementLogic
    {
        Task<List<AchievementViewModel>> GetList(long id);
        Task<AchievementViewModel> Get(long id);
        Task<bool> Create(AchievementBindingModel model);
        Task<bool> Update(AchievementBindingModel model);
        bool Delete(long id);
    }
}
