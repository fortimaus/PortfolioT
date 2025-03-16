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



        public async Task<List<Repository>> getInfoAsync(string link, HttpClient httpClient)
        {
            if (!checkLink(link))
                throw new Exception("Неверная сслыка на профиль GitHub");
            string userLogin = link.Replace(URL, "");
            
            List<Repository> repos = new List<Repository>();

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
                List<Repository> repositoryResponse = JsonSerializer.Deserialize<List<Repository>>(str);
                if(repositoryResponse != null)
                    repos.AddRange(repositoryResponse);

            }
            foreach(var repo in repos)
            {
                Console.WriteLine(repo.name);
                repo.branches = await getBranches(userLogin, repo.name, httpClient);
                
                foreach(var branch in repo.branches)
                {
                    Console.WriteLine("br-" + branch.name);
                    branch.commits = await getCommits(userLogin, repo.name, branch.name, httpClient);
                }

            }
            return repos;
        }

        public async Task<List<Branch>> getBranches(string userLogin, string repoName, HttpClient httpClient)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{userLogin}/{repoName}/branches?per_page=100");
            request.Headers.Add("User-Agent", "Mozilla Failfox 5.6");
            request.Headers.Add("Accept", "application/vnd.github+json");
            request.Headers.Add("Authorization", "token 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");

            using var response = await httpClient.SendAsync(request);

            string str = await response.Content.ReadAsStringAsync();
            List<Branch> repositoryResponse = JsonSerializer.Deserialize<List<Branch>>(str) ?? new List<Branch>();

            return repositoryResponse;
        }

        public async Task<List<Commit>> getCommits(string userLogin, string repoName, string branchName, HttpClient httpClient)
        {
            List<CommitSha> commitShaes = new List<CommitSha>();

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
                List<CommitSha> repositoryResponse = JsonSerializer.Deserialize<List<CommitSha>>(str) ?? new List<CommitSha>();

                commitShaes.AddRange(repositoryResponse);
            }
            List<Commit> commits = new List<Commit>();
            foreach(var sha in commitShaes)
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{userLogin}/{repoName}/commits/{sha.sha}");

                request.Headers.Add("User-Agent", "Mozilla Failfox 5.6");
                request.Headers.Add("Accept", "application/vnd.github+json");

                request.Headers.Add("Authorization", "token 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");

                using var response = await httpClient.SendAsync(request);

                string str = await response.Content.ReadAsStringAsync();

                Commit commitResponse = JsonSerializer.Deserialize<Commit>(str);
                
                Console.WriteLine(commitResponse.commit.message);
                commits.Add(commitResponse);

            }

            return commits;
        }
    }
}
