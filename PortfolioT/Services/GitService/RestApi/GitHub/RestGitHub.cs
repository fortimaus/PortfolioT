using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioT.Services.GitService.Models;
using PortfolioT.Services.GitService.RestApi.GitHub.Models;
using PortfolioT.Services.GitService.RestApi.GitHub.Models.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PortfolioT.Services.GitService.RestApi.GitHub
{
    class RestGitHub : IGitRestApi<GitHubRepository>
    {
        private int bathCommits = 15;
        private int bathRepository = 3;

        private string name = "GitHub";
        private string path_zip = @"C:\test_zips";
        private string API = "https://api.github.com";
        private string URL = "https://github.com/";
        public string Name
        {
            get => name;
        }
        private string Pattern
        {
            get => @"^\w*([-_.]\w+)*$";
        }

        private HttpClient httpClient;
        public RestGitHub()
        {
            httpClient = new HttpClient();
        }
        public async Task<bool> CheckUser(string userLogin)
        {
            if (!Regex.IsMatch(userLogin, Pattern, RegexOptions.IgnoreCase))
                return false;
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{API}/users/{userLogin}");
            
            foreach ((string, string) head in getHeaders())
                request.Headers.Add(head.Item1, head.Item2);
            
            using var response = await httpClient.SendAsync(request);
            return response.StatusCode.Equals(HttpStatusCode.OK);
        }

        private List<(string, string)> getHeaders()
        {
            List<(string, string)> headers = new List<(string, string)>
            {
                ("User-Agent", "Mozilla Failfox 5.6"),
                ("Accept", "application/vnd.github+json"),
                ("Authorization", "token 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")
            };
            return headers;
        }

        public async Task<List<GitHubRepository>> getManyReposAsync(string userLogin)
        {            
            List<GitHubRepository> repos = await getRepos(userLogin);
            
            

            List<GitHubRepository> resultRepos = new List<GitHubRepository>();
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
        private void prepareDictionary(string userLogin)
        {
            string path = @$"{path_zip}\{Name}\{userLogin}";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
        }
        public async Task<GitHubRepository> getOneRepoAsync(string userLogin, string repoName)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{API}/repos/{userLogin}/{repoName}");
            
            
            foreach ((string, string) head in getHeaders())
                request.Headers.Add(head.Item1, head.Item2);

            using var response = await httpClient.SendAsync(request);

            if (!response.StatusCode.Equals(HttpStatusCode.OK))
                return null;

            string str = await response.Content.ReadAsStringAsync();

            GitHubRepository repo = JsonSerializer.Deserialize<GitHubRepository>(str);

            prepareDictionary(userLogin);
            Task<string> zip_path = GetZip(userLogin, repoName);

            repo.link = URL + repo.full_name;

            Task<string> task_readme = getReadme(userLogin, repo.name, repo.default_branch);

            List<GitHubBranch> branches = await getBranches(userLogin, repo.name);

            HashSet<string> commits = new HashSet<string>();
            string user = userLogin.ToLower();

            foreach (var branch in branches)
            {
                List<GitHubCommitSha> shaes = await getCommitShaes(userLogin, repo.name, formatStringQuery(branch.name));

                foreach (GitHubCommitSha sha in shaes)
                {
                    if (!user.Equals(sha.CommitAuthor.ToLower()))
                        repo.teamwork = true;

                    commits.Add(sha.sha);
                }
            }

            List<GitHubCommit> list_commit = new List<GitHubCommit>();
            int pages = (int)Math.Ceiling((float)commits.Count / bathCommits);

            for (int i = 0; i < pages; i++)
            {
                var tasks = commits
                    .Skip(i * bathCommits).Take(bathCommits)
                    .Select(x => getCommit(userLogin, repo.name, x));

                list_commit.AddRange(await Task.WhenAll(tasks));
            }

            repo.commits = list_commit;
            repo.readme = await task_readme;
            repo.zip_path = await zip_path;
            if (repo.fork)
                repo.pullRequests = await getPRs(userLogin, repo.name);

            stopwatch.Stop();
            Console.WriteLine($"{repoName}\t{repo.commits.Count}\t{stopwatch.ElapsedMilliseconds / 1000} sec");

            return repo;
        }
        public async Task<string> GetZip(string userLogin, string repoName)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{API}/repos/{userLogin}/{repoName}/zipball");

            foreach ((string, string) head in getHeaders())
                request.Headers.Add(head.Item1, head.Item2);

            using var response = await httpClient.SendAsync(request);
            byte[] fileContent = await response.Content.ReadAsByteArrayAsync();
            string path = @$"{path_zip}\{Name}\{userLogin}\{repoName}.zip";
            await File.WriteAllBytesAsync(path, fileContent);
            return path;
        }
        private async Task<List<GitHubRepository>> getRepos(string userLogin)
        {
            List<GitHubRepository> repos = new List<GitHubRepository>();

            int page = 1;
            int last_page = 1;
            while (page <= last_page)
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"{API}/users/{userLogin}/repos?per_page=100&page={page}");

                foreach ((string, string) head in getHeaders())
                    request.Headers.Add(head.Item1, head.Item2);

                using var response = await httpClient.SendAsync(request);

                if (response.Headers.Contains("Link"))
                {
                    if (last_page == 1)
                    {
                        List<string> headerLinksString = response.Headers.GetValues("Link").ToList();
                        List<string> header_links = headerLinksString[0].Split(',').ToList();
                        foreach (string header_link in header_links)
                        {
                            if (header_link.Contains("rel=\"last\""))
                            {
                                Regex regexPage = new Regex(@"[?|&]{1}page=\d+");
                                Regex regexNumPage = new Regex(@"\d+");
                                Match match = regexPage.Match(header_link);
                                match = regexNumPage.Match(match.Value);
                                last_page = int.Parse(match.Value);
                            }
                        }
                    }
                }
                page += 1;

                string str = await response.Content.ReadAsStringAsync();
                List<GitHubRepository> repositoryResponse = JsonSerializer.Deserialize<List<GitHubRepository>>(str).Where(x => !x.empty).ToList() ?? new List<GitHubRepository>();
                repos.AddRange(repositoryResponse);

            }
            return repos;
        }
        public async Task<List<GitHubBranch>> getBranches(string userLogin, string repoName)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{API}/repos/{userLogin}/{repoName}/branches?per_page=100");

            foreach ((string, string) head in getHeaders())
                request.Headers.Add(head.Item1, head.Item2);


            using var response = await httpClient.SendAsync(request);

            string str = await response.Content.ReadAsStringAsync();
            List<GitHubBranch> repositoryResponse = JsonSerializer.Deserialize<List<GitHubBranch>>(str) ?? new List<GitHubBranch>();

            return repositoryResponse;
        }

        public string formatStringQuery(string value)
        {
            return value.Replace("&", "%26");
        }

        public async Task<List<GitHubCommitSha>> getCommitShaes(string userLogin, string repoName, string branchName)
        {
            List<GitHubCommitSha> commitShaes = new List<GitHubCommitSha>();
            int page = 1;

            int last_page = 1;

            while(page <= last_page)
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"{API}/repos/{userLogin}/{repoName}/commits?sha={branchName}&per_page=100&page={page}");


                foreach ((string, string) head in getHeaders())
                    request.Headers.Add(head.Item1, head.Item2);


                using var response = await httpClient.SendAsync(request);
                
                if(response.Headers.Contains("Link") && last_page == 1)
                {
                    List<string> headerLinksString = response.Headers.GetValues("Link").ToList();
                    List<string> header_links = headerLinksString[0].Split(',').ToList();
                    foreach (string header_link in header_links)
                    {
                        if (header_link.Contains("rel=\"last\""))
                        {
                            Regex regexPage = new Regex(@"[?|&]{1}page=\d+");
                            Regex regexNumPage = new Regex(@"\d+");
                            Match match = regexPage.Match(header_link);
                            match = regexNumPage.Match(match.Value);
                            last_page = int.Parse(match.Value);
                        }
                    }

                }

                page += 1;

                string str = await response.Content.ReadAsStringAsync();
                List<GitHubCommitSha> repositoryResponse = JsonSerializer.Deserialize<List<GitHubCommitSha>>(str) ?? new List<GitHubCommitSha>();

                commitShaes.AddRange(repositoryResponse);
            }
            

            return commitShaes;
        }

        public async Task<GitHubCommit> getCommit(string userLogin, string repoName, string sha)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{API}/repos/{userLogin}/{repoName}/commits/{sha}");

            foreach ((string, string) head in getHeaders())
                request.Headers.Add(head.Item1, head.Item2);


            using var response = await httpClient.SendAsync(request);

            string str = await response.Content.ReadAsStringAsync();
            GitHubCommit repositoryResponse = JsonSerializer.Deserialize<GitHubCommit>(str) ?? new GitHubCommit();
            return repositoryResponse;
        }
        public async Task<List<GitHubPullRequest>> getPRs(string userLogin, string repoName)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{API}/repos/{userLogin}/{repoName}/pulls?per_page=100&&state=closed");

            foreach ((string, string) head in getHeaders())
                request.Headers.Add(head.Item1, head.Item2);


            using var response = await httpClient.SendAsync(request);

            string str = await response.Content.ReadAsStringAsync();
            List<GitHubPullRequest> repositoryResponse = JsonSerializer.Deserialize<List<GitHubPullRequest>>(str) ?? new List<GitHubPullRequest>();

            return repositoryResponse;
        }

        private async Task<string> getReadme(string userLogin, string repoName, string branch)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://raw.githubusercontent.com/{userLogin}/{repoName}/{branch}/README.md");
            request.Headers.Add("User-Agent", "Mozilla Failfox 5.6");

            using var response = await httpClient.SendAsync(request);
            string str = await response.Content.ReadAsStringAsync();
            return str;
        }

       
    }
}
