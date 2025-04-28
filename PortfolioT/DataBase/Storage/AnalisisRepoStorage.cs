using Microsoft.EntityFrameworkCore;
using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Models;

namespace PortfolioT.DataBase.Storage
{
    public class AnalisisRepoStorage : IAnalisisRepoStorage
    {
        public AnalisisRepoViewModel GetAverage(long userId)
        {
            using var context = new DataBaseConnection();
            AnalisisRepoViewModel element = new AnalisisRepoViewModel();
            AnalisisUser? user = context.AnalisysUsers.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                return null;

            List<AnalisysRepo>? elements = context.Analisys
                .Include(x => x.user)
                .Where(x => x.userId == userId).ToList();

            element.userLink = user.link;
            
            element.scope_code = elements.Average(x => x.scope_code);
            element.scope_decor = elements.Average(x => x.scope_decor);

            element.scope_security = elements.Average(x => x.scope_security);
            element.scope_maintability = elements.Average(x => x.scope_maintability);
            element.scope_reability = elements.Average(x => x.scope_reability);

            return element;
        }

        public AnalisisRepoViewModel? GetOne( long userId, string repo)
        {
            using var context = new DataBaseConnection();
            AnalisysRepo? element = context.Analisys
                .Include(x => x.user)
                .FirstOrDefault(x =>  x.userId == userId && x.title.ToLower().Equals(repo.ToLower()));
            if (element == null)
                return null;
            return element.GetAnalisisRepoViewModel();
        }

        public List<AnalisisRepoViewModel>? GetList(long userId)
        {
            using var context = new DataBaseConnection();
            List<AnalisysRepo>? elements = context.Analisys
                .Include(x => x.user)
                .Where(x => x.userId == userId).ToList();
            return elements.Select(x => x.GetAnalisisRepoViewModel()).ToList();
        }

        public bool Create(AnalisisRepoBindingModel model)
        {
            using var context = new DataBaseConnection();
            AnalisisUser? user = context.AnalisysUsers.FirstOrDefault(x => x.Id == model.userId);
            if (user == null)
                throw new NullReferenceException("Не найден пользователь с заданным id");
            AnalisysRepo repo = new AnalisysRepo()
            {
                title = model.title,
                link = model.link,
                scope_code = model.scope_code,
                scope_decor = model.scope_decor,
                scope_maintability = model.scope_maintability,
                scope_reability = model.scope_reability,
                scope_security = model.scope_security,
                user = user,
                date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)

            };
            context.Analisys.Add(repo);
            context.SaveChanges();
            return true;
        }

        public bool CreateList(List<AnalisisRepoBindingModel> models)
        {
            using var context = new DataBaseConnection();
            List<AnalisysRepo> list = new List<AnalisysRepo>();
            foreach(AnalisisRepoBindingModel model in models)
            {
                AnalisisUser? user = context.AnalisysUsers.FirstOrDefault(x => x.Id == model.userId);
                if (user == null)
                    continue;
                AnalisysRepo repo = new AnalisysRepo()
                {
                    title = model.title,
                    link = model.link,
                    scope_code = model.scope_code,
                    scope_decor = model.scope_decor,
                    scope_maintability = model.scope_maintability,
                    scope_reability = model.scope_reability,
                    scope_security = model.scope_security,
                    user = user,
                    date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)

                };
                list.Add(repo);
            }
            
            context.Analisys.AddRange(list);
            context.SaveChanges();
            return true;
        }

        public bool Delete(long id)
        {
            using var context = new DataBaseConnection();
            AnalisysRepo? repo = context.Analisys.FirstOrDefault(x => x.Id == id);
            if (repo == null)
                return false;
            context.Remove(repo);
            context.SaveChanges();
            return true;
        }

        public bool DeleteMany(List<long> ids)
        {
            using var context = new DataBaseConnection();
            List<AnalisysRepo> repos = new List<AnalisysRepo>();
            foreach (var id in ids)
            {
                AnalisysRepo? repo = context.Analisys.FirstOrDefault(x => x.Id == id);
                if (repo == null)
                    continue;
                repos.Add(repo);
            }
            
            context.RemoveRange(repos);
            context.SaveChanges();
            return true;
        }
    }
}
