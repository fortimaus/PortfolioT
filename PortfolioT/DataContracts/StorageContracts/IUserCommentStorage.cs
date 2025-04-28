using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.StorageContracts
{
    public interface IUserCommentStorage
    {
        bool Create(UserCommentBindingModel model);
        List<UserCommentViewModel> UserComments(long userId);

    }
}
