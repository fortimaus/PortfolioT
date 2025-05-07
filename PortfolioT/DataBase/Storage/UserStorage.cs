using Microsoft.EntityFrameworkCore;
using PortfolioT.BusinessLogic.Exceptions;
using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;
using PortfolioT.DataModels.Models;
using System.ComponentModel;
using System.Xml.Linq;

namespace PortfolioT.DataBase.Storage
{
    public class UserStorage : IUserStorage
    {
        private FileSaver fileSaver;
        public UserStorage()
        {
            fileSaver = new FileSaver();
        }
        public async Task<long> Create(UserBindingModel model)
        {
            using var context = new DataBaseConnection();
            User newElement = new User()
            {
                login = model.login,
                password = model.password,
                email = model.email,
                role = UserRole.Non_Auth,
                status = model.status,
                about = model.about,
                code =  model.code
            };
            string path = fileSaver.prepareUsersDict();

            if (model.preview == null)
                newElement.preview = null;
            else
            {
                newElement.preview =  await fileSaver.savePreview(path, model.preview);
            }
            DeleteByLoginNonAuth(model.login);
            var element = context.Users.Add(newElement);
            context.SaveChanges();
            
            return element.Entity.Id;
        }
        private void DeleteByLoginNonAuth(string login)
        {
            using var context = new DataBaseConnection();
            User? user = context.Users
                .FirstOrDefault(x => x.login.Equals(login) && x.role == UserRole.Non_Auth);
            if (user != null)
                Delete(user.Id);
        }
        public async Task<UserViewModel> GetByLoginAndPassword(string login, string password)
        {
            using var context = new DataBaseConnection();
            User? user = context.Users
                .FirstOrDefault(x => x.login.Equals(login) && x.password.Equals(password) && x.role != UserRole.Non_Auth);
            if (user == null)
                throw new NullReferenceException("Пользователь не найден");
            return await user.GetViewModel();
        }
        public bool CheckByIdAndCode(long id, string code)
        {
            using var context = new DataBaseConnection();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                User? user = context.Users
                    .FirstOrDefault(x => x.Id == id);
                if (!user.code.Equals(code))
                {

                    return false;
                }
                user.code = null;
                context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Ошибка");
            }
            
        }
        public bool checkByLogin(string login)
        {
            using var context = new DataBaseConnection();
            User? user = context.Users
                .FirstOrDefault(x => x.login.Equals(login) && x.role != UserRole.Non_Auth);
            if (user != null)
                return false;
            return true;
        }

        public bool checkByEmail(string email)
        {
            using var context = new DataBaseConnection();
            User? user = context.Users.FirstOrDefault(x => x.email.Equals(email));
            if (user != null)
                return false;
            return true;
        }

        public bool Delete(long id)
        {
            using var context = new DataBaseConnection();
            User? user = context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
                throw new NullReferenceException("Пользователь не найден");
            else
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
                
            return true;
        }

        public async Task<List<UserViewModel>> FindByLogin(string search)
        {
            using var context = new DataBaseConnection();
            List<User> users = context.Users
                .Where(x => x.login.ToLower().Contains(search.ToLower()) && x.role != UserRole.Non_Auth)
                .ToList();
            List<UserViewModel> views = new List<UserViewModel>();
            foreach (var user in users)
                views.Add(await user.GetViewModel());
            return views;
        }

        public async Task<UserViewModel?> Get(long id)
        {
            using var context = new DataBaseConnection();
            User? user = context.Users.FirstOrDefault(x => x.Id == id);
            return  user == null ? null : await user.GetViewModel(); ;
        }

        public async Task<List<UserViewModel>> GetList()
        {
            using var context = new DataBaseConnection();
            List<User> users = context.Users.Where(x => x.role != UserRole.Non_Auth).ToList();
            List<UserViewModel> views = new List<UserViewModel>();
            foreach (var user in users)
                views.Add(await user.GetViewModel());
            return views;
        }
        public async Task<long> UpdateEmail(long id, string email)
        {
            using var context = new DataBaseConnection();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Users
                .FirstOrDefault(rec => rec.Id == id);
                if (element == null)
                    throw new NullReferenceException();
                element.email = email;

                context.SaveChanges();
                transaction.Commit();
                return element.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Ошибка при обновлении пользователя");
            }
        }
        public async Task<long> UpdateCode(long id, string code)
        {
            using var context = new DataBaseConnection();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Users
                .FirstOrDefault(rec => rec.Id == id);
                if (element == null)
                    throw new NullReferenceException();
                element.code = code;

                context.SaveChanges();
                transaction.Commit();
                return element.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Ошибка при обновлении пользователя");
            }
        }
        
        public async Task<bool> Update(UserBindingModel model)
        {
            using var context = new DataBaseConnection();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Users
                .FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                    throw new NullReferenceException();
                element.login = model.login;
                element.password = model.password;
                element.about = model.about;

                string path = fileSaver.prepareUsersDict();
                if (model.preview != null)
                {
                    if (element.preview == null)
                        element.preview = await fileSaver.savePreview(path, model.preview);
                    else
                        await File.WriteAllBytesAsync(@$"{element.preview}", model.preview);
                }

                context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch(NullReferenceException ex)
            {
                transaction.Rollback();
                throw new NullReferenceException("Пользователь не найден");
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Ошибка при обновлении пользователя");
            }
                
        }
        public bool UpdateRole(long userId, UserRole newRole)
        {
            using var context = new DataBaseConnection();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Users
                .FirstOrDefault(rec => rec.Id == userId);
                if (element == null)
                    throw new NullReferenceException();
                element.role = newRole;
                context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (NullReferenceException ex)
            {
                transaction.Rollback();
                throw new NullReferenceException("Пользователь не найден");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Ошибка при обновлении пользователя");
            }

        }
        public bool UpdateStatus(long userId, UserStatus newStatus)
        {
            using var context = new DataBaseConnection();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Users
                .FirstOrDefault(rec => rec.Id == userId);
                if (element == null)
                    throw new NullReferenceException();
                element.status = newStatus;
                context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (NullReferenceException ex)
            {
                transaction.Rollback();
                throw new NullReferenceException("Пользователь не найден");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Ошибка при обновлении пользователя");
            }

        }
    }
}
