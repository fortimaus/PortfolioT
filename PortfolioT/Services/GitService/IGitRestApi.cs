using PortfolioT.Services.GitService.Models;

namespace PortfolioT.Services.GitService
{
    public interface IGitRestApi<T>
        where T: IRepository
    {
        public Task<List<T>> getManyReposAsync(string userLogin);

        public Task<T> getOneRepoAsync(string userLogin, string repoName);

        public Task<bool> CheckUser(string userLogin);

        public string Name { get; }
    }
}
