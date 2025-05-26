using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortfolioT.DataBase;
using PortfolioT.Services.GitService;
using PortfolioT.Services.GitService.RestApi.Gitea;
using PortfolioT.Services.GitService.RestApi.Gitea.Models;
using PortfolioT.Services.GitService.RestApi.GitHub;
using PortfolioT.Services.GitService.RestApi.GitHub.Models;
using PortfolioT.Services.LibService.Models;
using PortfolioT.Services.LibService.Parsers;
namespace TestProject1
{
    class ServiceTest
    {
        RestGitea gitea = new RestGitea();
        RestGitHub gitHub = new RestGitHub();
        ElibUlstuParser elibUlstu = new ElibUlstuParser();

        string GitUlstuUser = "VoldemarProger";
        int countGutea = 5;
        string GitHubUser = "MaxKarme";
        int countGitHub = 5;
        string ElibData = "Романов-2020-2022";
        int countLib = 6;
        [Test]
        public async Task GiteaTest()
        {
            List<GiteaRepository> res = await gitea.getManyReposAsync(GitUlstuUser);
            Assert.That(countGutea, Is.EqualTo(res.Count()));
        }
        [Test]
        public async Task GitHubTest()
        {
            List<GitHubRepository> res = await gitHub.getManyReposAsync(GitHubUser);
            Assert.That(countGitHub, Is.EqualTo(res.Count()));
        }
        [Test]
        public async Task ElibTest()
        {
            List<Article> res = await elibUlstu.getArticles(ElibData);
            Assert.That(countLib, Is.EqualTo(res.Count()));
        }
    }
}
