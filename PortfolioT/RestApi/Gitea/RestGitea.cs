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

        

        public async Task<List<Repository>> getInfoAsync(string link, HttpClient httpClient)
        {
            if (!checkLink(link))
                throw new Exception("Неверная сслыка на профиль Git УлГТУ");
            
            string userLogin = link.Replace(URL, "");
            
            var response = await httpClient.GetAsync($"https://git.is.ulstu.ru/api/v1/users/{userLogin}/repos");
            
            string str = await response.Content.ReadAsStringAsync();
            
            List<Repository>? repos = JsonSerializer.Deserialize<List<Repository>>(str).Where(x => !x.empty).ToList();
            foreach (var rep in repos)
            {
                rep.branches = await getBranches(userLogin, rep.name, httpClient);
                foreach (var branch in rep.branches)   
                {
                    branch.commits = await getCommit(userLogin,rep.name,branch.name,httpClient);
                }
            }
            return repos;
        }

        public async Task<List<Branch>> getBranches(string user, string repo, HttpClient httpClient)
        {
            
            var response = await httpClient.GetAsync($"https://git.is.ulstu.ru/api/v1/repos/{user}/{repo}/branches");
            string str = await response.Content.ReadAsStringAsync();
            List<Branch> branches = JsonSerializer.Deserialize<List<Branch>>(str).ToList();
            return branches;
        }
        public async Task<List<Commit>> getCommit(string user, string repo, string branch, HttpClient httpClient)
        {
            var response = await httpClient.GetAsync($"https://git.is.ulstu.ru/api/v1/repos/{user}/{repo}/commits?sha={branch}");
            string str = await response.Content.ReadAsStringAsync();
            List<Commit> commits = JsonSerializer.Deserialize<List<Commit>>(str).ToList();
            return commits;
        }

    }
}
