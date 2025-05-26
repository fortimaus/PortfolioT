using HtmlAgilityPack;
using PortfolioT.Services.LibService.Models;
using System.IO;
using System.Text;

namespace PortfolioT.Services.LibService.Parsers
{
    public class ElibUlstuParser : IParser
    {
        private string URL_TOKEN = "https://elib.ulstu.ru/MegaPro/Web/Home";
        private string URL_CHANGE_DB = "https://elib.ulstu.ru/MegaPro/Web/Home/ChangeDb";
        private string URL_ARTICLE = "https://elib.ulstu.ru/MegaPro/Web/SearchResult/Ext";
        private string URL_PAGE = "https://elib.ulstu.ru/MegaPro/Web/SearchResult/ToPage/";
        private int bath_pages = 5;
        private HttpClient httpClient;

        public ElibUlstuParser()
        {
            httpClient = new HttpClient();
        }
        public async Task<List<Article>> getArticles(string info)
        {
            string[] lines = info.Split('-');

            string name = lines[0];
            string date_from = lines[1];
            string date_to = lines[2];

            using var requestCockie = new HttpRequestMessage(HttpMethod.Post, URL_TOKEN);

            requestCockie.Headers.Add("Accept", "*/*");
            requestCockie.Headers.Add("Host", "elib.ulstu.ru");
            requestCockie.Headers.Add("User-Agent", "Mozilla");

            using var responseCockie = await httpClient.SendAsync(requestCockie);
            string ASP_Cockie = null;
            if (responseCockie.Headers.Contains("Set-Cookie"))
            {
                 ASP_Cockie = responseCockie.Headers.GetValues("Set-Cookie")
                .Single(x => x.Contains("ASP.NET_SessionId"))
                .Split(' ').Single(x => x.Contains("ASP.NET_SessionId"));
            }
            //Console.WriteLine("Get Token");
            Dictionary<string, string> dataDb = new Dictionary<string, string>()
            {
                ["DbList"] = "7"
            };
            HttpContent contentDb = new FormUrlEncodedContent(dataDb);

            using var requestDb = new HttpRequestMessage(HttpMethod.Post, URL_CHANGE_DB);
            if(ASP_Cockie != null)
                requestDb.Headers.Add("Cookie", ASP_Cockie);
            requestDb.Headers.Add("Accept", "*/*");
            requestDb.Headers.Add("Host", "elib.ulstu.ru");
            requestDb.Headers.Add("User-Agent", "Mozilla");
            requestDb.Content = contentDb;

            using var responseChangeBd = await httpClient.SendAsync(requestDb);


            Dictionary<string, string> data = getContentSearch(name,date_from,date_to);
            HttpContent contentForm = new FormUrlEncodedContent(data);

            using var requestSearch = new HttpRequestMessage(HttpMethod.Post, URL_ARTICLE);
            if(ASP_Cockie != null)
                requestSearch.Headers.Add("Cookie", ASP_Cockie);
            requestSearch.Headers.Add("Accept", "*/*");
            requestSearch.Headers.Add("Host", "elib.ulstu.ru");
            requestSearch.Headers.Add("User-Agent", "Mozilla");
            requestSearch.Content = contentForm;

            using var responseSearch = await httpClient.SendAsync(requestSearch);
            
            

            Stream stream = responseSearch.Content.ReadAsStream();
            StreamReader sr = new StreamReader(stream);
            string html = sr.ReadToEnd();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNode pageNodes = doc.DocumentNode.SelectSingleNode("//*[@class=\"dtp-pager\"]");

            if (pageNodes == null)
                return getInfoPage(doc);
            else
            {
                List<Article> articles = new List<Article>();

                List<int> pageNums = pageNodes.SelectNodes(".//a")
                    .Select(x => int.Parse(x.Attributes["href"].Value))
                    .ToList();
                pageNums.RemoveAt(pageNums.Count-1);
                foreach (var item in pageNums)
                {
                    //Console.WriteLine(item);
                }
                int pages = (int)Math.Ceiling((float)pageNums.Count / bath_pages);
                List<HtmlDocument> docs = new List<HtmlDocument>() { doc };
                for (int i = 0; i < pages; i++)
                {
                    //Console.WriteLine($"Get page {i}");
                    var tasks = pageNums
                        .Skip(i * bath_pages)
                        .Take(bath_pages).Select(x => getHtmlFromPage(x, ASP_Cockie));
                    docs.AddRange(await Task.WhenAll(tasks));
                }
                foreach (HtmlDocument document in docs)
                    articles.AddRange(getInfoPage(document));
                return articles;
            }
                
        }
        private async Task<HtmlDocument> getHtmlFromPage(int page, string ASP_Cockie)
        {
            using var requestSearch = new HttpRequestMessage(HttpMethod.Post, $"{URL_PAGE}{page}");
            if(ASP_Cockie != null)
                requestSearch.Headers.Add("Cookie", ASP_Cockie);
            requestSearch.Headers.Add("Accept", "*/*");
            requestSearch.Headers.Add("Host", "elib.ulstu.ru");
            requestSearch.Headers.Add("User-Agent", "Mozilla");

            using var responseSearch = await httpClient.SendAsync(requestSearch);

            Stream stream = responseSearch.Content.ReadAsStream();
            StreamReader sr = new StreamReader(stream);
            string html = sr.ReadToEnd();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }
        private List<Article> getInfoPage(HtmlDocument doc)
        {
            List<Article> articles = new List<Article>();

            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*[@class=\"rs-data\"]");
            if (nodes == null)
                return articles;
            foreach(HtmlNode node in nodes)
            {
                
                Article article = new Article();
                (string, string) titleDesc = getTitleAndDesc(node);
                article.title = titleDesc.Item1;

                //Console.WriteLine($"Get article {titleDesc.Item1}");

                article.desc = titleDesc.Item2;
                article.authors = getAuthors(node);
                article.words = getWords(node);
                article.link = getLink(node);
                articles.Add(article);
            }

            return articles;
        }
        private (string, string) getTitleAndDesc(HtmlNode node)
        {
            HtmlNodeCollection descNodes = node.SelectNodes(".//div[1]/br/following-sibling::text()");

            string descStr = "";
            foreach (var descNode in descNodes)
                descStr += descNode.InnerText.Replace("\n", "");

            string[] descLines = descStr.Split("/", StringSplitOptions.RemoveEmptyEntries);

            return (descLines[0], descLines[descLines.Length - 1]);
        }
        private string getAuthors(HtmlNode node)
        {
            HtmlNode authorsNode = node.SelectSingleNode(".//div[2]");

            return authorsNode.InnerText;
        }
        private string getWords(HtmlNode node)
        {
            HtmlNode wordsNode = node.SelectSingleNode(".//div[3]");
            if (wordsNode == null)
                return "Ключевые слова:";
            return wordsNode.InnerText;
        }
        private string getLink(HtmlNode node)
        {
            HtmlNode linkNode = node.SelectSingleNode(".//div[5]/a");
            if (linkNode == null)
                return "Ссылка отсутствует";
            return linkNode.Attributes["href"].Value;
        }
        private Dictionary<string, string> getContentSearch(string name, string date_from, string date_to)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                ["bind_0"] = "0",
                ["bind_1"] = "0",
                ["bind_2"] = "0",
                ["dict_0"] = "6",
                ["dict_1"] = "31",
                ["dict_2"] = "31",
                ["dict_3"] = "6",
                ["cond_0"] = "1",
                ["cond_1"] = "5",
                ["cond_2"] = "6",
                ["cond_3"] = "0",
                ["term_0"] = name,
                ["term_1"] = date_from,
                ["term_2"] = date_to,
                ["term_3"] = "",
            };
            return dict;
        }
        
        
    }
}
