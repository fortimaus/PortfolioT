using PortfolioT.Analysis.Models;
using PortfolioT.Services.GitService.Models;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;

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

        private int points_for_commits_names = 5;
        private int points_for_size_commits = 10;
        private int sum_points_for_decor;

        private int points_for_PR = 10;

        private int good_point = 3;
        private int middle_point = 2;
        private int bad_point = 1;
        private int very_bad_point = 0;
        private float sum_points;

        public RepositoryAnalysis()
        {
            sum_points = good_point + middle_point + bad_point;
            sum_points_for_decor = points_for_commits_names + points_for_size_commits;
        }

        public List<ResponseRepository> analysisRepository(IEnumerable<IRepository> repos, string userLogin)
        {
            List<ResponseRepository> response = new List<ResponseRepository>();
            Console.WriteLine("анализ пошел");
            foreach (var repo in repos)
            {
                if (repo.empty)
                    continue;
                Console.WriteLine(repo.name);
                float scope_decor = 0;
                float scope_code = 0;
                float scope_bonus = 0;

                
                if (repo.teamwork)
                    (scope_decor,scope_bonus) = scopeForDecorationCommitManyUsers(repo.list_commits, userLogin);
                else
                    scope_decor += scopeForDecorationCommitSingleUser(repo.list_commits);

                scope_decor += checkDescAndReadme(repo.description, repo.readme);

                if (repo.fork && repo.list_pullRequests.Count() > 0)
                    scope_bonus += points_for_PR;
                response.Add(new ResponseRepository(
                    repo.name, repo.description, repo.link, repo.language,
                    scope_decor, scope_code, scope_bonus));
                Console.WriteLine(OpenZips(repo.zip_path));
                Console.WriteLine($"decor: {scope_decor} code: {scope_code} bonus: {scope_bonus}");
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
            scope_for_decor = scopeForDecorationCommitSingleUser(my_commits);
            scope_for_teamwork = 
                (scope_for_decor / sum_points_for_decor) * (my_changes_lines / (sum_changes_lines/users.Count)) * 10;
            return (scope_for_decor,scope_for_teamwork);
        }
        private float scopeForDecorationCommitSingleUser(IEnumerable<ICommit> commits)
        {
            float scope = 0;
            Dictionary<string, float> commit_words = new Dictionary<string, float>();
            float count_words = 0;
            List<float> commit_scopes = new List<float>();
            foreach(ICommit commit in commits)
            {
                string str = commit.message;
                str = Regex.Replace(str, "[-.?!)(,:;'[0-9\\]]", "");
                str.ToLower();
                string[] words = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    if (!commit_words.ContainsKey(word))
                        commit_words.Add(word, 1);
                    else
                        commit_words[word] += 1;
                    count_words += 1;
                }
                int commit_scope = codeEffAnalisys(commit.additions + commit.deletions)
                    + fileChangesAnalisys(commit.list_files.Count());     

                commit_scopes.Add(commit_scope / sum_points);
            }
            int count_groups = 0;
            foreach (var count in commit_words.Values)
            {
                if ((count / count_words) * 100 >= precent_groups_word)
                    count_groups++;
            }
            if (count_groups >= group_word_bound)
                scope += points_for_commits_names;
            scope += points_for_size_commits * (commit_scopes.Average());
            return scope;
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
        private int checkDescAndReadme(string desc, string readme)
        {
            return checkString(desc, limit_words_desc) + checkString(readme, limit_words_readme);
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
