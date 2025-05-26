using Microsoft.EntityFrameworkCore;
using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataBase.Storage
{
    public class UserCommentStorage : IUserCommentStorage
    {
        public bool Create(UserCommentBindingModel model)
        {
            using var context = new DataBaseConnection();

            User? user = context.Users.FirstOrDefault(x => x.Id == model.userId);
            User? moderator = context.Users.FirstOrDefault(x => x.Id == model.moderatorId);

            if (user == null || moderator == null)
                throw new NullReferenceException("Не найден пользователь с заданным id");

            UserComment newElement = new UserComment()
            {
                user = user,
                moderator = moderator,
                text = model.text,
            };
            context.UserComments.Add(newElement);
            context.SaveChanges();
            return true;
        }

        public async Task<List<UserCommentViewModel>> UserComments(long userId)
        {
            using var context = new DataBaseConnection();
            List<UserCommentViewModel> elements = new List<UserCommentViewModel>();
            foreach (var element in
                 context.UserComments.Include(x => x.moderator)
                 .Where(x => x.userId == userId).OrderByDescending(x => x.date))
            {
                var viewModel = element.GetUserCommentViewModel();
                if(element.moderator.preview != null)
                    viewModel.avatar = await File.ReadAllBytesAsync(element.moderator.preview);
                elements.Add(viewModel);
            }
            return elements;
        }
    }
}
