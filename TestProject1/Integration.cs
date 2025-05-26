using PortfolioT.BusinessLogic.Logics;
using PortfolioT.DataBase;
using PortfolioT.DataBase.Models;
using PortfolioT.DataBase.Storage;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.StorageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.Integration
{
    class Integration
    {
        

        UserStorage userStorage = new UserStorage();
        UserServiceStorage serviceStorage = new UserServiceStorage();
        RepoLogic repoLogic = new RepoLogic();
        AchievementLogic achievementLogic = new AchievementLogic();
        ArticleLogic articleLogic = new ArticleLogic();

        string[] names = new string[] { "vova", "dima", "vasya", "kolya", "masha" };
        string chars = "abcdefghijklmnopqrstuvwxyz";

        string GitUlstuUser = "VoldemarProger";
        int countGutea = 5;
        string GitHubUser = "fortimaus";
        int countGitHub = 1;
        string ElibData = "Романов-2020-2022";
        int countLib = 6;

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
        private void ClearAndAddServiceData()
        {
            using var context = new DataBaseConnection();
            var achs = context.UserServices.ToList();
            context.UserServices.RemoveRange(achs);
            context.SaveChanges();
            Random random = new Random();
            foreach (var user in context.Users.ToList())
            {
                serviceStorage.Create(new UserServiceBindingModel()
                {
                    userId = user.Id,
                    serviceId = 1,
                    data = GitHubUser
                });
                serviceStorage.Create(new UserServiceBindingModel()
                {
                    userId = user.Id,
                    serviceId = 2,
                    data = GitUlstuUser
                });
                serviceStorage.Create(new UserServiceBindingModel()
                {
                    userId = user.Id,
                    serviceId = 3,
                    data = ElibData
                });
            }
        }
        private void ClearReposAndArrticle()
        {
            using var context = new DataBaseConnection();
            var repos = context.Repos.ToList();
            context.Repos.RemoveRange(repos);
            var article = context.Articles.ToList();
            context.Articles.RemoveRange(article);
            context.SaveChanges();
        }
        [SetUp]
        public async Task cleanUp()
        {
            await ClearAndAddUsers();
            ClearAndAddServiceData();
            ClearReposAndArrticle();
        }
        [Test]
        public async Task GitHubTest()
        {
            using var context = new DataBaseConnection();
            User user = context.Users.First();
            await repoLogic.generateUserRepoByService(user.Id, 1);
            Assert.That(countGitHub, Is.EqualTo(context.Repos.Count()));
        }
        [Test]
        public async Task GitUlstuTest()
        {
            using var context = new DataBaseConnection();
            User user = context.Users.First();
            await repoLogic.generateUserRepoByService(user.Id, 2);
            Assert.That(countGutea, Is.EqualTo(context.Repos.Count()));
        }
        [Test]
        public async Task ElibTest()
        {
            using var context = new DataBaseConnection();
            User user = context.Users.First();
            await articleLogic.generateUserArticleByService(user.Id, 3);
            Assert.That(countLib, Is.EqualTo(context.Articles.Count()));
        }
    }
}
