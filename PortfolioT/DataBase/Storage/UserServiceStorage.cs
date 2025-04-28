using Microsoft.EntityFrameworkCore;
using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;

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
                throw new NullReferenceException("Не найдены данные для задания метки");

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
                throw new NullReferenceException("Не найдены данные сервиса");
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

        public List<UserServiceViewModel> GetUserListByService(long id, TypeService type)
        {
            using var context = new DataBaseConnection();
            return context.UserServices
                .Include(x => x.user)
                .Include(x => x.service)
                .Where(x => x.userId == id && x.service.type == type)
                .Select(x => x.GetViewModel()).ToList();
        }
        public UserServiceViewModel? GetUser(long userId, long serviceId)
        {
            using var context = new DataBaseConnection();
            UserService? user = context.UserServices
                .Include(x => x.user)
                .Include(x => x.service)
                .FirstOrDefault(x => x.userId == userId && x.serviceId == serviceId);
            if (user == null)
                return null;
            return user.GetViewModel();
                
        }

        public bool Update(UserServiceBindingModel model)
        {
            using var context = new DataBaseConnection();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                UserService? element = context.UserServices
                    .FirstOrDefault(x => x.userId == model.userId && x.serviceId == model.serviceId);
                if (element == null)
                    throw new NullReferenceException();
                element.data = model.data;
                context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch(NullReferenceException ex)
            {
                transaction.Rollback();
                throw new NullReferenceException("Не найдены данные сервиса");
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw new Exception("Ошибка обновления данных");
            }
        }
    }
}
