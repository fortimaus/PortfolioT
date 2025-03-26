using PortfolioT.RestApi.GitHub.Models;
using PortfolioT.RestApi.GitHub.Models.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace PortfolioT.RestApi.GitHub
{
    class RestGitHub
    {
        private string name = "GitHub";
        private string URL = "https://github.com/";
        public string Name
        {
            get => name;
        }
        public string Pattern
        {
            get => @$"^{URL}\w*([-_.]\w+)*$";
        }

        public bool checkLink(string link)
        {
            return Regex.IsMatch(link, Pattern, RegexOptions.IgnoreCase);
        }



        public async Task<List<GitHubRepository>> getInfoAsync(string link, HttpClient httpClient)
        {
            if (!checkLink(link))
                throw new Exception("Неверная сслыка на профиль GitHub");
            string userLogin = link.Replace(URL, "");
            
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
                    if(last_page==1)
                    {
                        List<string> headerLinksString = response.Headers.GetValues("Link").ToList();
                        List<string> header_links = headerLinksString[0].Split(',').ToList();
                        foreach (string header_link in header_links)
                        {
                            if(header_link.Contains("rel=\"last\""))
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
                if(repositoryResponse != null)
                    repos.AddRange(repositoryResponse);

            }
            foreach(var repo in repos)
            {
                repo.link = URL + repo.full_name;
                List<GitHubBranch> branches = await getBranches(userLogin, repo.name, httpClient);
                HashSet<string> commits = new HashSet<string>();
                foreach (var branch in branches)
                {
                    List<GitHubCommitSha> shaes = await getCommitShaes(userLogin, repo.name, formatStringQuery(branch.name), httpClient);
                    foreach (GitHubCommitSha sha in shaes)
                    {
                        commits.Add(sha.sha);
                    }
                }
                repo.commits = await getCommits(userLogin, repo.name, commits, httpClient);
                
                if (repo.fork)
                    repo.pullRequests = await getPRs(userLogin, repo.name, httpClient);

            }
            return repos;
        }

        public async Task<List<GitHubBranch>> getBranches(string userLogin, string repoName, HttpClient httpClient)
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

        public async Task<List<GitHubCommitSha>> getCommitShaes(string userLogin, string repoName, string branchName, HttpClient httpClient)
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

        public async Task<List<GitHubCommit>> getCommits(string userLogin, string repoName, IEnumerable<string> shaes, HttpClient httpClient)
        {
            List<GitHubCommit> commits = new List<GitHubCommit>();
            foreach(string sha in shaes)
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{userLogin}/{repoName}/commits/{sha}");
                request.Headers.Add("User-Agent", "Mozilla Failfox 5.6");
                request.Headers.Add("Accept", "application/vnd.github+json");
                request.Headers.Add("Authorization", "token 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");

                using var response = await httpClient.SendAsync(request);

                string str = await response.Content.ReadAsStringAsync();
                GitHubCommit repositoryResponse = JsonSerializer.Deserialize<GitHubCommit>(str) ?? new GitHubCommit();
                commits.Add(repositoryResponse);
            }
            

            return commits;
        }
        public async Task<List<GitHubPullRequest>> getPRs(string userLogin, string repoName, HttpClient httpClient)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{userLogin}/{repoName}/pulls?per_page=100");
            request.Headers.Add("User-Agent", "Mozilla Failfox 5.6");
            request.Headers.Add("Accept", "application/vnd.github+json");
            request.Headers.Add("Authorization", "token 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");

            using var response = await httpClient.SendAsync(request);

            string str = await response.Content.ReadAsStringAsync();
            List<GitHubPullRequest> repositoryResponse = JsonSerializer.Deserialize<List<GitHubPullRequest>>(str) ?? new List<GitHubPullRequest>();

            return repositoryResponse;
        }
    }
}
