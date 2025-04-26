using Microsoft.EntityFrameworkCore;
using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataBase.Storage
{
    public class UserServiceStorage : IUserServiceStorage
    {
        public bool Create(UserServiceBindingModel model)
        {
            using var context = new DataBaseConnection();

            User? user = context.Users.FirstOrDefault(x => x.Id == model.userId);
            Service? service = context.Services.FirstOrDefault(x => x.Id == model.serviceId);
            
            if (user == null || service == null)
                return false;
            
            UserService userService = new UserService() 
            {
                user = user,
                service = service,
                data = model.data
            };
            context.UserServices.Add(userService);
            context.SaveChanges();
            return true;
        }

        public bool Delete(UserServiceBindingModel model)
        {
            using var context = new DataBaseConnection();
            UserService? userService = context.UserServices
                .Include(x => x.user)
                .Include(x => x.service)
                .FirstOrDefault(x => x.userId == model.userId && x.serviceId == model.serviceId);
            if (userService == null)
                return false;
            context.UserServices.Remove(userService);
            context.SaveChanges();
            return true;
        }

        
        public List<UserServiceViewModel> GetUserList(long id)
        {
            using var context = new DataBaseConnection();            
            return context.UserServices
                .Include(x => x.user)
                .Include(x => x.service)
                .Where(x => x.userId == id)
                .Select(x => x.GetViewModel()).ToList();
        }
    }
}
