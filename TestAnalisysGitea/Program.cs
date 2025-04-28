// See https://aka.ms/new-console-template for more information
using PortfolioT.DataBase;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.Services.GitService;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using TestS.Commons;
using TestS.DB;

HttpClient httpClient = new HttpClient();
using DBConnection context = new DBConnection();

var response = await httpClient.GetAsync($"https://git.is.ulstu.ru/api/v1/users/search");
string str = await response.Content.ReadAsStringAsync();

SearchResponse searchResponse = JsonSerializer.Deserialize<SearchResponse>(str) ?? new SearchResponse();
DBclear();

Console.WriteLine("Стартуем");
Stopwatch watchOver = new Stopwatch();

int bath = 5;
int pages = (int)Math.Ceiling((float)searchResponse.data.Count() / bath);

watchOver.Start();
for (int i = 0; i < pages; i++)
{
    
    var tasks = searchResponse.data
        .Skip(i * bath)
        .Take(bath).Select(x => analisys(x.login));
    await Task.WhenAll(tasks);
}

watchOver.Stop();
int users = context.Users.Count();
int repos = context.Users.Sum(x => x.count_repo) ?? 0;
Console.WriteLine("Финиш:");
Console.WriteLine($"Общее время сбора: {watchOver.ElapsedMilliseconds / 1000} sec");
Console.WriteLine($"Кол-во обработанных пользователей: {users}");
Console.WriteLine($"Кол-во обработанных репозриториев: {repos}");

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
        Console.WriteLine(name);
        GitService gitService = new GitService();
        Stopwatch watchInner = new Stopwatch();
        watchInner.Start();

        var repositories = await gitService.GetUserWorksByService(new UserServiceBindingModel()
        {
            serviceId = InitServices.GitUlstu.Id,
            data = name
        });

        watchInner.Stop();
        createEn(name, repositories.Count, watchInner.ElapsedMilliseconds / 1000);

        return repositories.Count;
    }
    catch
    {
        return 0;
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