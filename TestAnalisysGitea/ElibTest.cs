using PortfolioT.Services.LibService.Models;
using PortfolioT.Services.LibService.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestS
{
    class ElibTest
    {
        ElibUlstuParser elibUlstu = new ElibUlstuParser();
        List<string> datas = new List<string>()
        {
            "Романов-2010-2025",
            "Науменко-2021-2025",
            "Шарапов-2000-2025",
            "Вельмисов-2000-2025",
            "Орлов-2000-2025",
            "Ямлеева-2000-2025",
            "Волгин-2000-2025",
        };
        public async Task run()
        {
            Console.WriteLine("Start");
            int count = 0;
            List<float> average_Time = new List<float>();

            Stopwatch watchOver = new Stopwatch();
            watchOver.Start();

            Stopwatch watchInner = new Stopwatch();
            Console.WriteLine($"Данные\t\t\tКол-во\t\t\tВремя");
            foreach (var item in datas)
            {
                
                watchInner.Restart();
                List<Article> articles = await elibUlstu.getArticles(item);
                watchInner.Stop();
                count += articles.Count;

                float time = watchInner.ElapsedMilliseconds;
                Console.WriteLine($"{item}\t\t{articles.Count} статей\t\t{time/1000} сек");
                average_Time.Add((float)articles.Count/time);
            }
            watchOver.Stop();
            Console.WriteLine("Finish");
            Console.WriteLine($"Кол-во собранных статей: {count} статей");
            Console.WriteLine($"Общее время: {watchOver.ElapsedMilliseconds/1000}  сек");
            Console.WriteLine($"Среднее время на сбор одной статьи: {(average_Time.Average())/1000} сек");
        }
    }
}
