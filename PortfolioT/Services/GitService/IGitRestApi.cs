using PortfolioT.Services.GitService.Models;
using System.Threading.Tasks.Sources;
using Microsoft.Build.Framework;
using System.Collections.Generic;
using PortfolioT.Services.GitService.RestApi.GitHub.Models;

namespace PortfolioT.Services.GitService
{
    public interface IGitRestApi<T>
        where T: IRepository
    {
        public Task<List<T>> getManyReposAsync(string userLogin);

        public Task<T> getOneRepoAsync(string userLogin, string repoName);

        public Task<bool> CheckUser(string userLogin);

        public Task<List<T>> getReposInfo(string userLogin);

        public string Name { get; }
    }
}
