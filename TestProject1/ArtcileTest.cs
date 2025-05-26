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
    class ArtcileTest
    {
        UserStorage userStorage = new UserStorage();
        ArticleStorage articleStorage = new ArticleStorage();
        string[] names = new string[] { "vova", "dima", "vasya", "kolya", "masha" };
        string[] titles = new string[] { "science", "language", "compramition", "learnin", "data science" };
        string chars = "abcdefghijklmnopqrstuvwxyz";
        int allCountArts = 0;
        Dictionary<long, int> users_article = new Dictionary<long, int>();
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
        private async Task ClearAndAddArticle()
        {
            using var context = new DataBaseConnection();
            var arts = context.Articles.ToList();
            context.Articles.RemoveRange(arts);
            context.SaveChanges();
            Random random = new Random();
            users_article.Clear();
            foreach (var user in context.Users.ToList())
            {
                users_article.Add(user.Id, 0);
                for (int i = 0; i < random.Next(3, 10); i++)
                {
                    try
                    {
                        await articleStorage.Create(new ArticleBindingModel()
                        {
                            title = titles[random.Next(0, titles.Length)],
                            description = "desc",
                            link = "link",
                            userId = user.Id,
                            words = "words",
                            serviceId = 3
                        });
                        allCountArts++;
                        users_article[user.Id]++;
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
            await ClearAndAddArticle();
        }
        [Test]
        public async Task createTest()
        {
            using var context = new DataBaseConnection();
            Assert.That(allCountArts, Is.EqualTo(context.Articles.Count()));
        }
        [Test]
        public async Task GetByUsersTest()
        {
            using var context = new DataBaseConnection();
            foreach (var user in context.Users.ToList())
            {
                int userCount = (await articleStorage.GetList(user.Id)).Count();

                Assert.That(users_article[user.Id], Is.EqualTo(userCount));
            }

        }

        [Test]
        public async Task UpdateTest()
        {
            using var context = new DataBaseConnection();
            Random random = new Random();
            List<Article> users = context.Articles.ToList();
            foreach (var item in users)
            {
                await articleStorage.Update(new ArticleBindingModel()
                {
                    Id = item.Id,
                    title = item.title + " Update",
                    description = "desc",
                    link = "",
                    words = "words"
                });
            }
            Assert.That(context.Articles.All(x => x.title.Contains("Update")));
        }
        [Test]
        public void DeleteTest()
        {
            using var context = new DataBaseConnection();
            Random random = new Random();
            int startCount = context.Articles.Count();
            int countDelete = random.Next(0, 5);
            List<Article> DeleteAchs = context.Articles.Take(countDelete).ToList();
            foreach (var item in DeleteAchs)
            {
                articleStorage.Delete(item.Id);
            }
            Assert.That(startCount - countDelete, Is.EqualTo(context.Articles.Count()));
        }
    }
}
