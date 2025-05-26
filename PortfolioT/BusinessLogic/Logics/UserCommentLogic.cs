using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Models;

namespace PortfolioT.BusinessLogic.Logics
{
    public class UserCommentLogic : IUserCommentLogic
    {
        UserCommentStorage commentStorage;
        public UserCommentLogic()
        {
            commentStorage = new UserCommentStorage();
        }
        public bool Create(UserCommentBindingModel model)
        {
            try
            {
                return commentStorage.Create(model);
                
            }
            catch
            {
                return false;
                throw;
            }
        }

        public async Task<List<UserCommentViewModel>> UserComments(long userId)
        {
            try
            {
                return await commentStorage.UserComments(userId);

            }
            catch
            {
                throw;
            }
        }
    }
}
