using PortfolioT.RestApi.Gitea.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.Gitea
{
    class RestGitea
    {
        private string name = "GitUlstu";
        private string URL = "https://git.is.ulstu.ru/";
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

        

        public async Task<List<GiteaRepository>> getInfoAsync(string link, HttpClient httpClient)
        {
            if (!checkLink(link))
                throw new Exception("Неверная сслыка на профиль Git УлГТУ");
            
            string userLogin = link.Replace(URL, "");
            
            var response = await httpClient.GetAsync($"https://git.is.ulstu.ru/api/v1/users/{userLogin}/repos");
            
            string str = await response.Content.ReadAsStringAsync();
            
            List<GiteaRepository>? repos = JsonSerializer.Deserialize<List<GiteaRepository>>(str).Where(x => !x.empty).ToList();
            foreach (var rep in repos)
            {
                rep.link = URL + rep.full_name;
                List<GiteaBranch> branches = await getBranches(userLogin, rep.name, httpClient);
                Dictionary<string, GiteaCommit> commits = new Dictionary<string, GiteaCommit>();
                foreach (var branch in branches)   
                {
                    List<GiteaCommit> branchCommits = await getCommit(userLogin,rep.name,branch.name,httpClient);
                    foreach(GiteaCommit commit in branchCommits)
                    {
                        if (!commits.ContainsKey(commit.sha))
                            commits.Add(commit.sha, commit);
                    }
                }
                rep.commits = commits.Values.ToList();
                if (rep.fork)
                    rep.pullRequests = await getPRs(userLogin, rep.name, httpClient); 
            }

            return repos;
        }

        public async Task<List<GiteaBranch>> getBranches(string user, string repo, HttpClient httpClient)
        {
            
            var response = await httpClient.GetAsync($"https://git.is.ulstu.ru/api/v1/repos/{user}/{repo}/branches");
            string str = await response.Content.ReadAsStringAsync();
            List<GiteaBranch> branches = JsonSerializer.Deserialize<List<GiteaBranch>>(str).ToList();
            return branches;
        }
        public async Task<List<GiteaCommit>> getCommit(string user, string repo, string branch, HttpClient httpClient)
        {
            var response = await httpClient.GetAsync($"https://git.is.ulstu.ru/api/v1/repos/{user}/{repo}/commits?sha={branch}");
            string str = await response.Content.ReadAsStringAsync();
            List<GiteaCommit> commits = JsonSerializer.Deserialize<List<GiteaCommit>>(str).ToList();
            return commits;
        }

        public async Task<List<GiteaPullRequest>> getPRs(string user, string repo, HttpClient httpClient)
        {
            var response = await httpClient.GetAsync($"https://git.is.ulstu.ru/api/v1/repos/{user}/{repo}/pulls?state=closed");
            string str = await response.Content.ReadAsStringAsync();
            List<GiteaPullRequest> prs = JsonSerializer.Deserialize<List<GiteaPullRequest>>(str).ToList();
            return prs;
        }

    }
}
