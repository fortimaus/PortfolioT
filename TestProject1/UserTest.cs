using PortfolioT.DataBase.Storage;
using PortfolioT.DataBase;
using PortfolioT.DataContracts.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioT.DataBase.Models;
using PortfolioT.DataModels.Enums;

namespace TestProject1
{
    class UserTest
    {
        UserStorage userStorage = new UserStorage();
        string[] names = new string[] { "vova", "dima", "vasya", "kolya", "masha" };
        string chars = "abcdefghijklmnopqrstuvwxyz";
        int users_count = 0;
        [SetUp]
        public async Task cleanUp()
        {
            users_count = 0;
            using var context = new DataBaseConnection();
            var users = context.Users.ToList();
            context.Users.RemoveRange(users);
            context.SaveChanges();
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    await userStorage.Create(new UserBindingModel()
                    {
                        login = $"{names[random.Next(0, names.Length)]}{random.Next(0, 50)}",
                        password = $"{random.Next(10, 100)}{random.Next(10, 100)}{random.Next(10, 100)}",
                        email = $"{chars[random.Next(0, chars.Length)]}{chars[random.Next(0, chars.Length)]}{random.Next(0, 100)}@gmail.com",
                        code = "0000"
                    });
                    users_count++;
                }
                catch
                {
                    i--;
                    continue;
                }
            }
        }
        [Test]
        public async Task createTest()
        {
            using var context = new DataBaseConnection();
            Random random = new Random();            
            Assert.That(users_count, Is.EqualTo(context.Users.Count()));
        }

        [Test]
        public async Task UpdateTest()
        {
            using var context = new DataBaseConnection();
            Random random = new Random();
            List<User> users = context.Users.ToList();
            foreach (var item in users)
            {
                await userStorage.Update(new UserBindingModel()
                {
                    Id = item.Id,
                    login = item.login + " Update",
                    password = item.password,
                    email = item.email
                });
            }
            Assert.That(context.Users.All(x => x.login.Contains("Update")));
        }
        [Test]
        public void DeleteTest()
        {
            using var context = new DataBaseConnection();
            Random random = new Random();
            int countDelete = random.Next(0, 10);
            List<User> DeleteUsers = context.Users.Take(countDelete).ToList();
            foreach (var item in DeleteUsers)
            {
                userStorage.Delete(item.Id);
            }
            Assert.That(10-countDelete, Is.EqualTo(context.Users.Count()));
        }

        [Test]
        public void GetByLoginAndPasswordTest()
        {
            using var context = new DataBaseConnection();
            Random random = new Random();
            var existUser = context.Users.First();
            var user = userStorage.GetByLoginAndPassword(existUser.login, existUser.password);
            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public void UpdateRoleTest()
        {
            using var context = new DataBaseConnection();
            Random random = new Random();
            Dictionary<UserRole, int> roles_count = new Dictionary<UserRole, int>()
            {
                { UserRole.User,0},
                { UserRole.Moderator,0},
                { UserRole.Admin,0},
            };
            
            foreach (var item in context.Users.ToList())
            {
                Array values = Enum.GetValues(typeof(UserRole));
                UserRole newRole = (UserRole)values.GetValue(random.Next(2,values.Length));
                userStorage.UpdateRole(item.Id, newRole);
                roles_count[newRole]++;
            }
            var roles = context.Users.GroupBy(p => p.role)
                    .Select(g => new { role = g.Key, Count = g.Count() });
            foreach (var user_role in roles)
            {
                Assert.That(roles_count[user_role.role], Is.EqualTo(user_role.Count));
            }
        }

        [Test]
        public void UpdatStatusTest()
        {
            using var context = new DataBaseConnection();
            Random random = new Random();
            Dictionary<UserStatus, int> statuses_count = new Dictionary<UserStatus, int>()
            {
                { UserStatus.Confirm,0},
                { UserStatus.Warnings,0},
                { UserStatus.Critical,0},
            };

            foreach (var item in context.Users.ToList())
            {
                Array values = Enum.GetValues(typeof(UserStatus));
                UserStatus newStatus = (UserStatus)values.GetValue(random.Next(1, values.Length));
                userStorage.UpdateStatus(item.Id, newStatus);
                statuses_count[newStatus]++;
            }
            var statuses = context.Users.GroupBy(p => p.status)
                    .Select(g => new { status = g.Key, Count = g.Count() });
            foreach (var user_status in statuses)
            {
                Assert.That(statuses_count[user_status.status], Is.EqualTo(user_status.Count));
            }
        }

    }
}
