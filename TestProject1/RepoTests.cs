using PortfolioT.DataBase.Models;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataBase;
using PortfolioT.DataContracts.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    class RepoTests
    {
        UserStorage userStorage = new UserStorage();
        RepoStorage repoStorage = new RepoStorage();
        string[] names = new string[] { "vova", "dima", "vasya", "kolya", "masha" };
        string[] titles = new string[] { "mobile", "web", "java", "python", "c#" };
        string chars = "abcdefghijklmnopqrstuvwxyz";
        int allCountRepos = 0;
        Dictionary<long, int> users_repos = new Dictionary<long, int>();
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
        private async Task ClearAndAddRepos()
        {
            using var context = new DataBaseConnection();
            var achs = context.Repos.ToList();
            context.Repos.RemoveRange(achs);
            context.SaveChanges();
            Random random = new Random();
            users_repos.Clear();
            foreach (var user in context.Users.ToList())
            {
                users_repos.Add(user.Id, 0);
                for (int i = 0; i < random.Next(3, 10); i++)
                {
                    try
                    {
                        await repoStorage.Create(new RepoBindingModel()
                        {
                            title = titles[random.Next(0, titles.Length)],
                            description = "desc",
                            link = "link",
                            userId = user.Id,
                            language = "language",
                            scope_decor = random.Next(0,30),
                            scope_code = random.Next(0, 70),
                            scope_team = random.Next(0, 20),
                            scope_maintability = random.Next(0, 5),
                            scope_reability = random.Next(0, 5),
                            scope_security = random.Next(0, 5),
                            comments = "comments",
                            serviceId = 1,
                            date = DateOnly.FromDateTime(DateTime.Now).ToString()
                        });
                        allCountRepos++;
                        users_repos[user.Id]++;
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
            await ClearAndAddRepos();
        }
        [Test]
        public async Task createTest()
        {
            using var context = new DataBaseConnection();
            Assert.That(allCountRepos, Is.EqualTo(context.Repos.Count()));
        }
        [Test]
        public async Task GetByUsersTest()
        {
            using var context = new DataBaseConnection();
            foreach (var user in context.Users.ToList())
            {
                int userCount = (await repoStorage.GetList(user.Id)).Count();

                Assert.That(users_repos[user.Id], Is.EqualTo(userCount));
            }

        }

        [Test]
        public async Task UpdateTest()
        {
            using var context = new DataBaseConnection();
            Random random = new Random();
            List<Repo> users = context.Repos.ToList();
            foreach (var item in users)
            {
                await repoStorage.Update(new RepoBindingModel()
                {
                    Id = item.Id,
                    title = item.title + " Update",
                    description = "desc",
                    link = "",
                    language = "lang"
                });
            }
            Assert.That(context.Repos.All(x => x.title.Contains("Update")));
        }
        [Test]
        public void DeleteTest()
        {
            using var context = new DataBaseConnection();
            Random random = new Random();
            int startCount = context.Repos.Count();
            int countDelete = random.Next(0, 5);
            List<Repo> DeleteRepos = context.Repos.Take(countDelete).ToList();
            foreach (var item in DeleteRepos)
            {
                repoStorage.Delete(item.Id);
            }
            Assert.That(startCount - countDelete, Is.EqualTo(context.Repos.Count()));
        }
    }
}
