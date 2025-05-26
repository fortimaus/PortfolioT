using PortfolioT.DataBase;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.Services.GitService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestS.DB;

namespace TestS
{
    class GitUlstuTest
    {
        GitService gitService = new GitService();
        public async Task run()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };


            HttpClient httpClient = new HttpClient(clientHandler);

            
            using DBConnection context = new DBConnection();

            var response = await httpClient.GetAsync($"http://git.is.ulstu.ru/api/v1/users/search");
            string str = await response.Content.ReadAsStringAsync();

            SearchResponse searchResponse = JsonSerializer.Deserialize<SearchResponse>(str) ?? new SearchResponse();
            DBclear();

            Console.WriteLine("Стартуем");
            Stopwatch watchOver = new Stopwatch();
            watchOver.Start();

            int i = 0;
            int all = searchResponse.data.Take(50).Count();
            foreach (var user in searchResponse.data.Take(50))
            {

                Console.Clear();
                Console.WriteLine($"page {i}/{all} ||| {((float)i / all) * 100}%");
                Console.WriteLine(user.login);
                await analisys(user.login);

                i++;
            }
            watchOver.Stop();


            int users = context.Users.Count();
            int repos = context.Users.Sum(x => x.count_repo) ?? 0;
            Console.WriteLine("Финиш:");
            Console.WriteLine($"Общее время сбора: {watchOver.ElapsedMilliseconds / 1000} sec");
            Console.WriteLine($"Кол-во обработанных пользователей: {users}");
            Console.WriteLine($"Кол-во обработанных репозриториев: {repos}");

        }
        void DBclear()
        {
            using DBConnection context = new DBConnection();
            context.Users.RemoveRange(context.Users);
            context.SaveChanges();
        }
        void createEn(string name, int repos, long time)
        {
            using DBConnection context = new DBConnection();
            User user = new User()
            {
                name = name,
                count_repo = repos,
                time = time
            };
            context.Users.Add(user);
            context.SaveChanges();
        }
        async Task<int> analisys(string name)
        {
            try
            {

                Stopwatch watchInner = new Stopwatch();
                watchInner.Start();

                var repositories = await gitService.GetUserWorksByService(new UserServiceBindingModel()
                {
                    serviceId = InitServices.GitUlstu.Id,
                    data = name
                });

                watchInner.Stop();
                Console.Write($" {watchInner.ElapsedMilliseconds / 1000} sec \n");
                createEn(name, repositories.Count, watchInner.ElapsedMilliseconds / 1000);

                return repositories.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
                Console.Write($" Error: {ex.Message}\n");
                return 0;
            }

        }
    }
    class SearchResponse
    {
        public List<Data> data { get; set; } = new List<Data>();
    }
    class Data
    {
        public string login { get; set; } = string.Empty;
    }
}
