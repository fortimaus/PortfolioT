using PortfolioT.DataBase.Models;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataBase;
using PortfolioT.DataContracts.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioT.DataModels.Models;

namespace TestProject1
{
    class AchievementTest
    {
        UserStorage userStorage = new UserStorage();
        AchievementStorage achievementStorage = new AchievementStorage();
        string[] names = new string[] { "vova", "dima", "vasya", "kolya", "masha" };
        string[] titles = new string[] { "competition", "exhibition", "meeting", "training", "camp" };
        string chars = "abcdefghijklmnopqrstuvwxyz";
        int allCountAch = 0;
        Dictionary<long, int> users_achievements = new Dictionary<long, int>();
        private async Task ClearAndAddUsers()
        {
            using var context = new DataBaseConnection();
            var users = context.Users.ToList();
            context.Users.RemoveRange(users);
            context.SaveChanges();
            Random random = new Random();
            for (int i = 0; i < 3; i++)
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
                }
                catch
                {
                    i--;
                    continue;
                }
            }
        }
        private async Task ClearAndAddAchievements()
        {
            using var context = new DataBaseConnection();
            var achs = context.Achievements.ToList();
            context.Achievements.RemoveRange(achs);
            context.SaveChanges();
            Random random = new Random();
            users_achievements.Clear();
            foreach (var user in context.Users.ToList())
            {
                users_achievements.Add(user.Id, 0);
                for (int i = 0; i < random.Next(3, 10); i++)
                {
                    try
                    {
                        await achievementStorage.Create(new AchievementBindingModel()
                        {
                            title = titles[random.Next(0, titles.Length)],
                            description = "desc",
                            link = "link",
                            userId = user.Id,
                        });
                        allCountAch++;
                        users_achievements[user.Id]++;
                    }
                    catch
                    {
                        i--;
                        continue;
                    }
                    
                }

            }
        }
        [SetUp]
        public async Task cleanUp()
        {
            await ClearAndAddUsers();
            await ClearAndAddAchievements();
        }
        [Test]
        public async Task createTest()
        {
            using var context = new DataBaseConnection();
            Assert.That(allCountAch, Is.EqualTo(context.Achievements.Count()));
        }
        [Test]
        public async Task GetByUsersTest()
        {
            using var context = new DataBaseConnection();
            foreach (var user in context.Users.ToList())
            {
                int userCount = (await achievementStorage.GetList(user.Id)).Count();

                Assert.That(users_achievements[user.Id], Is.EqualTo(userCount));
            }
            
        }

        [Test]
        public async Task UpdateTest()
        {
            using var context = new DataBaseConnection();
            Random random = new Random();
            List<Achievement> users = context.Achievements.ToList();
            foreach (var item in users)
            {
                await achievementStorage.Update(new AchievementBindingModel()
                {
                    Id = item.Id,
                    title = item.title + " Update",
                    description = "desc",
                    link = ""
                });
            }
            Assert.That(context.Achievements.All(x => x.title.Contains("Update")));
        }
        [Test]
        public void DeleteTest()
        {
            using var context = new DataBaseConnection();
            Random random = new Random();
            int startCount = context.Achievements.Count();
            int countDelete = random.Next(0, 5);
            List<Achievement> DeleteAchs = context.Achievements.Take(countDelete).ToList();
            foreach (var item in DeleteAchs)
            {
                achievementStorage.Delete(item.Id);
            }
            Assert.That(startCount- countDelete, Is.EqualTo(context.Achievements.Count()));
        }

    }
}
