using PortfolioT.Analysis.Models;
using PortfolioT.RestApi.Gitea.Models;
using System.Linq;

namespace PortfolioT.Analysis
{
    public class RepositoryAnalysis
    {
        public List<ResponseRepository> analysisGiteaRepository(List<GiteaRepository> repos)
        {
            List<ResponseRepository> response = new List<ResponseRepository>();

            

            foreach(var repo in repos)
            {
                Dictionary<string, SummaryInfo> usersWork = new Dictionary<string, SummaryInfo>();
                    foreach(var commit in repo.commits)
                    {
                        string author = string.Empty;
                        if (commit.author == null)
                            author = commit.commit.author.email;
                        else
                            author = commit.author.login;


                        SummaryInfo info;

                        if (!usersWork.ContainsKey(author.ToLower()))
                        {
                            info = new SummaryInfo();
                            usersWork.Add(author.ToLower(), info);
                        }
                        
                        info = usersWork[author.ToLower()];
                        
                        info.addDate(commit.sha, commit.created);
                        foreach (var file in commit.files)
                        {
                            switch (file.status)
                            {
                                case "added":
                                    info.addAdditions();
                                    break;
                                case "modified":
                                    info.addModefieds();
                                    break;
                                case "removed":
                                    info.addDeletions();
                                    break;
                            }
                    }
                        
                    
                }
                Console.WriteLine(repo.name);
                Console.WriteLine(usersWork.Count);
                if(usersWork.Count == 1)
                {
                    SummaryInfo info = usersWork.Values.ToArray()[0];
                    ResponseRepository responseRepository = new ResponseRepository(
                        repo.name, repo.description, repo.created_at, repo.updated_at,
                        agregateSingleInfo(info), info.additions, info.modified, info.deletions
                        );
                    response.Add(responseRepository);
                }
                else 
                {
                    Console.WriteLine("TeamWork");
                }

            }

            return response;
        }

         public double agregateSingleInfo(SummaryInfo info)
        {
            List<double> diffDatesInHours = new List<double>();

            List<DateTime> dates = info.dates.Values.ToList();

            dates.Sort();
            dates.Reverse();
            for(int i = 0; i < dates.Count-1; i++)
            {
                Console.WriteLine(dates.ElementAt(i));
                diffDatesInHours.Add(
                    (dates.ElementAt(i)- dates.ElementAt(i+1)).TotalHours
                    );
                Console.WriteLine($"{i}-{(dates.ElementAt(i) - dates.ElementAt(i + 1)).TotalHours} ");
            }
            double dateMediana = 0;
            diffDatesInHours.Sort();
            if (diffDatesInHours.Count == 0)
                return dateMediana;
            if (diffDatesInHours.Count % 2 == 0)
                dateMediana = (diffDatesInHours.ElementAt(diffDatesInHours.Count / 2) + diffDatesInHours.ElementAt((diffDatesInHours.Count / 2) - 1)) / 2;
            else
                dateMediana = diffDatesInHours.ElementAt(diffDatesInHours.Count / 2);
            Console.WriteLine("med - " + dateMediana);

            return dateMediana;

        }
    }
}
