using Microsoft.Extensions.DependencyInjection;
using PortfolioT.DataBase;
using PortfolioT.Services.GitService.RestApi.Gitea.Models;
using PortfolioT.Services.GitService.RestApi.GitHub.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortfolioT.Services.GitService.RestApi.Gitea
{
    class RestGitea : IGitRestApi<GiteaRepository>
    {
        private int bathRepository = 3;

        private string name = InitServices.GitUlstu.title;
        private string path_zip = @"C:\test_zips";
        private string API = "https://git.is.ulstu.ru/api/v1";
        public static string URL = "https://git.is.ulstu.ru/";
        public string Name
        {
            get => name;
        }
        public string Pattern
        {
            get => @$"^{URL}\w*([-_.]\w+)*$";
        }
        private static HttpClient httpClient = new HttpClient();

        public async Task<bool> CheckUser(string userLogin)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{API}/users/{userLogin}");
            using var response = await httpClient.SendAsync(request);
            return response.StatusCode.Equals(HttpStatusCode.OK);
        }

        private void prepareDictionary(string userLogin)
        {
            string path = @$"{path_zip}\{Name}\{userLogin}";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
        }

        public async Task<List<GiteaRepository>> getReposInfo(string userLogin)
        {
            var response = await httpClient.GetAsync($"{API}/users/{userLogin}/repos");

            string str = await response.Content.ReadAsStringAsync();
            List<GiteaRepository>? repos = JsonSerializer.Deserialize<List<GiteaRepository>>(str).Where(x => !x.empty).ToList();

            return repos;
        }
        public async Task<List<GiteaRepository>> getManyReposAsync(string userLogin)
        {
            
            var response = await httpClient.GetAsync($"{API}/users/{userLogin}/repos");
            
            string str = await response.Content.ReadAsStringAsync();
            Stopwatch stopwatch = new Stopwatch();

            List<GiteaRepository>? repos = JsonSerializer.Deserialize<List<GiteaRepository>>(str).Where(x => !x.empty).ToList();
            
            List<GiteaRepository> resultRepos = new List<GiteaRepository>();
            int pages = (int)Math.Ceiling((float)repos.Count / bathRepository);

            for (int i = 0; i < pages; i++)
            {                
                var tasks = repos
                    .Skip(i * bathRepository).Take(bathRepository)
                    .Select(x => getOneRepoAsync(userLogin, x.name));

                resultRepos.AddRange(await Task.WhenAll(tasks));
                
            }
            
            return resultRepos;
        }
        public async Task<GiteaRepository> getOneRepoAsync(string userLogin, string repoName)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{API}/repos/{userLogin}/{repoName}");
            using var response = await httpClient.SendAsync(request);

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                return null;

            string str = await response.Content.ReadAsStringAsync();

            GiteaRepository rep = JsonSerializer.Deserialize<GiteaRepository>(str);

            prepareDictionary(userLogin);
            Task<string> path_zip = getZip(userLogin, repoName, rep.defaultBranch);

            rep.link = URL + rep.full_name;
            rep.readme = await getReadme(userLogin, rep.name);
            List<GiteaBranch> branches = await getBranches(userLogin, rep.name);

            Dictionary<string, GiteaCommit> commits = new Dictionary<string, GiteaCommit>();

            string user = string.Empty;

            foreach (var branch in branches)
            {
                List<GiteaCommit> branchCommits = await getCommit(userLogin, rep.name, branch.name);
                foreach (GiteaCommit commit in branchCommits)
                {
                    if (user.Length == 0)
                        user = commit.commitAuthor.ToLower();
                    else if (!user.Equals(commit.commitAuthor.ToLower()))
                        rep.teamwork = true;

                    if (!commits.ContainsKey(commit.sha))
                        commits.Add(commit.sha, commit);
                }
            }
            rep.commits = commits.Values.ToList();
            rep.zip_path = await path_zip;
            if (rep.fork)
                rep.pullRequests = await getPRs(userLogin, rep.name);
            stopwatch.Stop();
            //Console.WriteLine($"{repoName}\t{rep.commits.Count}\t{stopwatch.ElapsedMilliseconds / 1000} sec");
            return rep;
        }
        private async Task<string> getZip(string userLogin, string repoName, string branch)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{API}/repos/{userLogin}/{repoName}/archive/{branch}.zip");

            using var response = await httpClient.SendAsync(request);
            byte[] fileContent = await response.Content.ReadAsByteArrayAsync();
            
            Random random = new Random();
            string uuid = $"{random.Next(0,10)}{random.Next(0, 10)}{random.Next(0, 10)}{random.Next(0, 10)}";
            
            string path = @$"{path_zip}\{Name}\{userLogin}\{repoName}{uuid}.zip";
            await File.WriteAllBytesAsync(path, fileContent);
            return path;
        }
        public async Task<List<GiteaBranch>> getBranches(string user, string repo)
        {
            
            var response = await httpClient.GetAsync($"{API}/repos/{user}/{repo}/branches");
            string str = await response.Content.ReadAsStringAsync();
            List<GiteaBranch> branches = JsonSerializer.Deserialize<List<GiteaBranch>>(str).ToList();
            return branches;
        }
        public async Task<List<GiteaCommit>> getCommit(string user, string repo, string branch)
        {
            var response = await httpClient.GetAsync($"{API}/repos/{user}/{repo}/commits?sha={branch}");
            string str = await response.Content.ReadAsStringAsync();
            List<GiteaCommit> commits = JsonSerializer.Deserialize<List<GiteaCommit>>(str).ToList();
            return commits;
        }

        public async Task<List<GiteaPullRequest>> getPRs(string user, string repo)
        {
            var response = await httpClient.GetAsync($"{API}/repos/{user}/{repo}/pulls?state=closed");
            string str = await response.Content.ReadAsStringAsync();
            List<GiteaPullRequest> prs = JsonSerializer.Deserialize<List<GiteaPullRequest>>(str).ToList();
            return prs;
        }
        public async Task<string> getReadme(string user, string repo)
        {
            var response = await httpClient.GetAsync($"{API}/repos/{user}/{repo}/raw/README.md");
            string str = await response.Content.ReadAsStringAsync();
            return str;
        }

        
    }
}
