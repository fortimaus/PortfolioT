using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.BusinessLogicsContracts
{
    public interface IUserCommentLogic
    {
        bool Create(UserCommentBindingModel model);
        Task<List<UserCommentViewModel>> UserComments(long userId);
    }
}
