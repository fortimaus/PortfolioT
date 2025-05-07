using Microsoft.Extensions.FileSystemGlobbing.Internal;
using PortfolioT.Analysis.Models;
using PortfolioT.Analysis.Models.XmlCommon;
using PortfolioT.Services.GitService.Models;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml;

namespace PortfolioT.Analysis
{
    public class RepositoryAnalysis
    {
        private int limit_words_desc = 5;
        private int limit_words_readme = 15;
        private int points_for_desc_readme = 5;

        private int group_word_bound = 3;
        private int precent_groups_word = 25;

        private int code_lower_bound = 5;
        private int code_midle_bound = 300;
        private int code_lot_of_bound = 2000;

        private int file_lower_bound = 3;
        private int file_midle_bound = 7;
        private int file_lot_of_bound = 15;

        private int points_for_commits_names = 10;
        private int points_for_size_commits = 10;
        private int sum_points_for_decor;

        private int points_for_code = 70;

        private int points_for_PR = 10;

        private int good_point = 3;
        private int middle_point = 2;
        private int bad_point = 1;
        private int very_bad_point = 0;
        private float sum_points;

        private int bath_repos = 3;
        private int bath_get_result = 10;

        private int bath_remove = 5;

        private SonarQubeScanner sonar;
        private HttpClient httpClient;
        public RepositoryAnalysis()
        {
            httpClient = new HttpClient();
            sum_points = good_point + good_point;
            sum_points_for_decor = points_for_commits_names + points_for_size_commits;
            sonar = new SonarQubeScanner(httpClient);
        }

        public async Task<List<ResponseRepository>> analysisRepository(IEnumerable<IRepository> repos, string userLogin)
        {
            List<ResponseRepository> response = new List<ResponseRepository>();
            Dictionary<int, IRepository> dict_repos = new Dictionary<int, IRepository>();
            int i = 0;
            //Console.WriteLine("Start Ananlisys");

            foreach (var repo in repos)
            {
                if (repo.empty)
                    continue;
                //Console.WriteLine(repo.name);
                float scope_decor = 0;
                float scope_bonus = 0;

                string comments = string.Empty;

                if (repo.teamwork)
                    (scope_decor, scope_bonus) = scopeForDecorationCommitManyUsers(repo.list_commits, userLogin);
                else
                    (scope_decor,comments) = scopeForDecorationCommitSingleUser(repo.list_commits);

                float scope_desc_readme = 0;
                (scope_desc_readme, comments) = checkDescAndReadme(repo.description, repo.readme);

                scope_decor += scope_desc_readme;
                repo.comments += comments;

                if (repo.fork && repo.list_pullRequests.Count() > 0)
                    scope_bonus += points_for_PR;
                repo.dir_path = OpenZips(repo.zip_path);

                repo.scope_decor = scope_decor;
                repo.scope_bonus = scope_bonus;

                dict_repos.Add(i, repo);
                i++;
            }

            int pages = (int)Math.Ceiling((float)dict_repos.Keys.Count / bath_repos);
            
            for (i = 0; i < pages; i++)
            {
                var tasks = dict_repos.Values
                    .Skip(i * bath_repos)
                    .Take(bath_repos).Select(x => sonar.analisysCode
                        (x.language ?? "", x.name, x.dir_path));
                await Task.WhenAll(tasks);
            }

            List<AnalisysResponse> results = new List<AnalisysResponse>();
            //Console.WriteLine("Get results analisys");
            pages = (int)Math.Ceiling((float)dict_repos.Keys.Count / bath_get_result);
            for (i = 0; i < pages; i++)
            {
                var tasks = dict_repos
                    .Skip(i * bath_get_result)
                    .Take(bath_get_result).Select(x => sonar.getResultAnalisys
                        (x.Key, x.Value.name));
                results.AddRange(await Task.WhenAll(tasks));
            }
            foreach (AnalisysResponse result in results)
            {
                response.Add(new ResponseRepository(
                    dict_repos[result.id].name, dict_repos[result.id].description,
                    dict_repos[result.id].link, dict_repos[result.id].language,
                    dict_repos[result.id].scope_decor, result.scope_cof * points_for_code, dict_repos[result.id].scope_bonus,
                    result.scope_security, result.scope_maintability, result.scope_reability,
                    DateTime.ParseExact(dict_repos[result.id].updated_at.Split('T')[0], "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
                    dict_repos[result.id].comments + result.comments));
            }
            
            foreach(var repo in dict_repos.Values)
            {
                deleteZip(repo.zip_path);
                deleteDir(repo.dir_path);
            }

            pages = (int)Math.Ceiling((float)response.Count / bath_remove);
            for (i = 0; i < pages; i++)
            {
                var tasks = response
                    .Skip(i * bath_remove)
                    .Take(bath_remove).Select(x => sonar.deleteProjects(x.title));
                await Task.WhenAll(tasks);
            }
            return response;
        }


        private (float, float) scopeForDecorationCommitManyUsers(IEnumerable<ICommit> commits, string userLogin)
        {
            float scope_for_decor = 0;
            float scope_for_teamwork = 0;
            List<ICommit> my_commits = new List<ICommit>();
            float sum_changes_lines = 0;
            float my_changes_lines = 0;
            HashSet<string> users = new HashSet<string>();
            foreach (var commit in commits)
            {
                float commits_lines = commit.additions + commit.deletions;
                if (commits_lines > code_lot_of_bound)
                    commits_lines = 1;
                users.Add(commit.commitAuthor.ToLower());
                if (commit.commitAuthor.ToLower().Equals(userLogin.ToLower()))
                {
                    my_commits.Add(commit);
                    my_changes_lines += commits_lines;
                }
                sum_changes_lines += commits_lines;
            }
            if (my_commits.Count == 0)
                return (0, 0);
            (scope_for_decor, string test) = scopeForDecorationCommitSingleUser(my_commits);
            scope_for_teamwork =
                ((scope_for_decor / sum_points_for_decor) + (my_changes_lines / sum_changes_lines)) * 10;
            return (scope_for_decor, scope_for_teamwork);
        }
        private (float, string) scopeForDecorationCommitSingleUser(IEnumerable<ICommit> commits)
        {
            float scope = 0;
            string comments = string.Empty;
            Dictionary<string, float> commit_words = new Dictionary<string, float>();
            List<float> commit_scopes = new List<float>();
            foreach (ICommit commit in commits)
            {
                string str = commit.message;
                str = Regex.Replace(str, "[-.?!)(,:;'[0-9\\]]", "");
                str.ToLower();
                string[] words = str.Split(' ', StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                foreach (var word in words)
                {
                    if (!commit_words.ContainsKey(word))
                        commit_words.Add(word, 1);
                    else
                        commit_words[word] += 1;
                }
                int commit_scope = codeEffAnalisys(commit.additions + commit.deletions)
                    + fileChangesAnalisys(commit.list_files.Count());

                commit_scopes.Add(commit_scope / sum_points);
            }
            int count_groups = 0;
            foreach (var count in commit_words.Values)
            {
                if ((count / commits.Count()) * 100 >= precent_groups_word)
                    count_groups++;
            }
            if (count_groups >= group_word_bound)
                scope += points_for_commits_names;
            else
                comments += "Страйтесь писать названия коммитов по единому шаблону";
            scope += points_for_size_commits * (commit_scopes.Average());
            return (scope, comments);
        }
        private int codeEffAnalisys(int countChanges)
        {
            if (countChanges < code_lower_bound)
                return middle_point;
            else if (countChanges >= code_lower_bound && countChanges <= code_midle_bound)
                return good_point;
            else if (countChanges > code_midle_bound && countChanges <= code_lot_of_bound)
                return bad_point;
            else
                return very_bad_point;
        }
        private int fileChangesAnalisys(int countFiles)
        {
            if (countFiles <= file_lower_bound)
                return good_point;
            else if (countFiles > file_lower_bound && countFiles <= file_midle_bound)
                return middle_point;
            else if (countFiles > file_midle_bound && countFiles <= file_lot_of_bound)
                return bad_point;
            else
                return very_bad_point;
        }
        private (float, string) checkDescAndReadme(string desc, string readme)
        {
            int readme_scope = checkString(readme, limit_words_readme);
            int desc_scope = checkString(desc, limit_words_desc);
            string comments = string.Empty;
            if (readme_scope == 0)
                comments += "Добавить README \n";
            if (desc_scope == 0)
                comments += "Добавить краткое описание проекта";
            return (readme_scope + desc_scope, comments);
        }
        private int checkString(string value, int limitaion)
        {
            if (value == null || value.Length == 0)
                return 0;
            string[] lines = value.Split('\n');
            int count_words = 0;
            foreach (string line in lines)
                count_words += line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            if (count_words >= limitaion)
                return points_for_desc_readme;
            else
                return 0;
        }

        private bool deleteZip(string path)
        {
            try
            {
                FileInfo fileInf = new FileInfo(path);
                if (fileInf.Exists)
                    fileInf.Delete();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        private bool deleteDir(string path)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                if (dirInfo.Exists)
                    dirInfo.Delete(true);
                return true;
            }
            catch
            {
                return false;
            }

        }
        private string OpenZips(string path_to_zip)
        {
            FileInfo file = new FileInfo(path_to_zip);
            DirectoryInfo directory = file.Directory;
            using ZipArchive zip = ZipFile.Open(file.FullName, ZipArchiveMode.Read);
            string zipDir = @$"{directory.FullName}\{zip.Entries[0].FullName.Replace("/", "")}";
            if (Directory.Exists(zipDir))
                Directory.Delete(zipDir, true);
            ZipFile.ExtractToDirectory(file.FullName, directory.FullName);
            return zipDir;
        }

        
    }
}
