using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioT.RestApi.GitHub.Models;
using PortfolioT.RestApi.GitHub.Models.Support;
using PortfolioT.RestApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PortfolioT.RestApi.GitHub
{
    class RestGitHub
    {
        private int bathCommits = 15;

        private string name = "GitHub";
        private string path_zip = @"C:\test_zips\";
        private string URL = "https://github.com/";
        public string Name
        {
            get => name;
        }
        public string Pattern
        {
            get => @$"^{URL}\w*([-_.]\w+)*$";
        }

        private HttpClient httpClient;
        public RestGitHub()
        {
            httpClient = new HttpClient();
        }
        public bool checkLink(string link)
        {
            return Regex.IsMatch(link, Pattern, RegexOptions.IgnoreCase);
        }



        public async Task<List<GitHubRepository>> getInfoAsync(string link)
        {
            if (!checkLink(link))
                throw new Exception("Неверная сслыка на профиль GitHub");
            string userLogin = link.Replace(URL, "");
            
            List<GitHubRepository> repos = await getRepos(userLogin);

            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Repo \t\t\t commits \t\t\t time");
            foreach (var repo in repos)
            {
                stopwatch.Reset();
                stopwatch.Start();
                repo.link = URL + repo.full_name;
                repo.readme = await getReadme(userLogin, repo.name, repo.default_branch);
                List<GitHubBranch> branches = await getBranches(userLogin, repo.name);
                HashSet<string> commits = new HashSet<string>();
                string user = string.Empty;
                foreach (var branch in branches)
                {
                    List<GitHubCommitSha> shaes = await getCommitShaes(userLogin, repo.name, formatStringQuery(branch.name));
                    foreach (GitHubCommitSha sha in shaes)
                    {
                        if (user.Length == 0)
                            user = sha.author.ToLower();
                        else if (!user.Equals(sha.author.ToLower()))
                            repo.teamwork = true;

                        commits.Add(sha.sha);
                    }
                }

                List<GitHubCommit> list_commit = new List<GitHubCommit>();
                int pages = (int)Math.Ceiling((float)commits.Count/ bathCommits);
                
                for(int i = 0; i < pages; i++)
                {
                    var tasks = commits
                        .Skip(i * bathCommits).Take(bathCommits)
                        .Select(x => getCommit(userLogin, repo.name, x));

                    list_commit.AddRange(await Task.WhenAll(tasks));
                }
                
                repo.commits = list_commit;
                if (repo.fork)
                    repo.pullRequests = await getPRs(userLogin, repo.name);

                stopwatch.Stop();
                Console.WriteLine($"{repo.name}\t\t\t{repo.commits.Count}\t\t\t{stopwatch.ElapsedMilliseconds / 1000} sec");
            }
            return repos;
        }

        public async Task<string> GetZip(string userLogin, string repoName)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{userLogin}/{repoName}/zipball");
            request.Headers.Add("User-Agent", "Mozilla Failfox 5.6");
            request.Headers.Add("Accept", "application/vnd.github+json");
            request.Headers.Add("Authorization", "token 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");

            using var response = await httpClient.SendAsync(request);
            byte[] fileContent = await response.Content.ReadAsByteArrayAsync();
            await System.IO.File.WriteAllBytesAsync($"{path_zip}{repoName}.zip", fileContent);
            return "Yes";
        }
        private async Task<List<GitHubRepository>> getRepos(string userLogin)
        {
            List<GitHubRepository> repos = new List<GitHubRepository>();

            int page = 1;
            int last_page = 1;
            while (page <= last_page)
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/users/{userLogin}/repos?per_page=100&page={page}");
                request.Headers.Add("User-Agent", "Mozilla Failfox 5.6");
                request.Headers.Add("Accept", "application/vnd.github+json");
                request.Headers.Add("Authorization", "token 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");

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
                List<GitHubRepository> repositoryResponse = JsonSerializer.Deserialize<List<GitHubRepository>>(str);
                if (repositoryResponse != null)
                    repos.AddRange(repositoryResponse);

            }
            return repos;
        }
        public async Task<List<GitHubBranch>> getBranches(string userLogin, string repoName)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{userLogin}/{repoName}/branches?per_page=100");
            request.Headers.Add("User-Agent", "Mozilla Failfox 5.6");
            request.Headers.Add("Accept", "application/vnd.github+json");
            request.Headers.Add("Authorization", "token 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");

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
                using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{userLogin}/{repoName}/commits?sha={branchName}&per_page=100&page={page}");

                request.Headers.Add("User-Agent", "Mozilla Failfox 5.6");
                request.Headers.Add("Accept", "application/vnd.github+json");
                request.Headers.Add("Authorization", "token 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");

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
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{userLogin}/{repoName}/commits/{sha}");
            request.Headers.Add("User-Agent", "Mozilla Failfox 5.6");
            request.Headers.Add("Accept", "application/vnd.github+json");
            request.Headers.Add("Authorization", "token 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");

            using var response = await httpClient.SendAsync(request);

            string str = await response.Content.ReadAsStringAsync();
            GitHubCommit repositoryResponse = JsonSerializer.Deserialize<GitHubCommit>(str) ?? new GitHubCommit();
            return repositoryResponse;
        }
        public async Task<List<GitHubPullRequest>> getPRs(string userLogin, string repoName)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{userLogin}/{repoName}/pulls?per_page=100&&state=closed");
            request.Headers.Add("User-Agent", "Mozilla Failfox 5.6");
            request.Headers.Add("Accept", "application/vnd.github+json");
            request.Headers.Add("Authorization", "token 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");

            using var response = await httpClient.SendAsync(request);

            string str = await response.Content.ReadAsStringAsync();
            List<GitHubPullRequest> repositoryResponse = JsonSerializer.Deserialize<List<GitHubPullRequest>>(str) ?? new List<GitHubPullRequest>();

            return repositoryResponse;
        }

        public async Task<string> getReadme(string userLogin, string repoName, string branch)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://raw.githubusercontent.com/{userLogin}/{repoName}/{branch}/README.md");
            request.Headers.Add("User-Agent", "Mozilla Failfox 5.6");

            using var response = await httpClient.SendAsync(request);
            string str = await response.Content.ReadAsStringAsync();
            return str;
        }
    }
}
