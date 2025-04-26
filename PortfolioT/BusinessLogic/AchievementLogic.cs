using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.BusinessLogic
{
    public class AchievementLogic : IAchievementLogic
    {
        AchievementStorage achievementStorage;

        public AchievementLogic()
        {
            achievementStorage = new AchievementStorage();
        }
        public async Task<bool> Create(AchievementBindingModel model)
        {
            if (!validate(model))
                return false;
            return await achievementStorage.Create(model);
        }

        public bool Delete(long id)
        {
            return achievementStorage.Delete(id);
        }

        public async Task<AchievementViewModel?> Get(long id)
        {
            return await achievementStorage.Get(id);
        }

        public async Task<List<AchievementViewModel>> GetList(long id)
        {
            return await achievementStorage.GetList(id);
        }

        public async Task<bool> Update(AchievementBindingModel model)
        {
            if (validate(model))
                return false;
            return await achievementStorage.Update(model);
        }

        private bool validate(AchievementBindingModel model)
        {
            if (model.title.Length == 0)
                return false;
            if (model.description.Length == 0)
                return false;
            return true;
        }
    }
}
