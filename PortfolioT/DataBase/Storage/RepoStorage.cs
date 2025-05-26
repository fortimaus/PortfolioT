using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using PortfolioT.Controllers.Commons;
using PortfolioT.DataBase.Commons;
using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataBase.Storage
{
    public class RepoStorage : IRepoStorage
    {
        private readonly string NAME = "Repo";
        private FileSaver fileSaver;
        private ImageStorage imageStorage;
        public RepoStorage()
        {
            fileSaver = new FileSaver();
            imageStorage = new ImageStorage();
        }
        public async Task<bool> Create(RepoBindingModel model)
        {
            using var context = new DataBaseConnection();
            User? user = context.Users.FirstOrDefault(x => x.Id == model.userId);
            Service? service = context.Services.FirstOrDefault(x => x.Id == model.serviceId);
            if (user == null)
                throw new NullReferenceException("Не найден пользователь с заданным id");
            Repo newElement = new Repo()
            {
                title = model.title,
                description = model.description,
                link = model.link,
                user = user,
                language = model.language,
                scope_decor = model.scope_decor,
                scope_code = model.scope_code,
                scope_team = model.scope_team,
                scope_maintability = model.scope_maintability,
                scope_reability = model.scope_reability,
                scope_security = model.scope_security,
                comments = model.comments,
                date = DateOnly.Parse(model.date),
                service = service,
                basic = false
            };

            string path = fileSaver.prepareDictionary(NAME, user.Id);

            if (model.preview == null)
                newElement.preview = null;
            else
            {
                string file_name = await fileSaver.savePreview(path, model.preview);
                newElement.preview = @$"{path}\{file_name}";
            }

            var element = context.Achievements.Add(newElement);

            context.SaveChanges();
            if (model.images != null)
            {
                await imageStorage.Create(context, model.images.Select(x => x.image).ToList(), path, NAME, element.Entity.Id);
            }
            return true;
        }
        public int countRepoByUser(long userId)
        {
            using var context = new DataBaseConnection();
            int count = context.Repos.Count() > 0 ?
                context.Repos
                .Where(x => x.userId == userId).Count() : 0;
            return count;
        }
        public int AverageCountRepo()
        {
            using var context = new DataBaseConnection();
            int count = context.Repos.Count() > 0 ?
                (int)context.Repos.GroupBy(x => x.userId).ToList().Average(x => x.Count()) : 0;
            return count;
        }
        public CompareUserRepoInfo GetAverage(long userId)
        {
            using var context = new DataBaseConnection();
            CompareUserRepoInfo element = new CompareUserRepoInfo();
            List<Repo>? elements = context.Repos
                .Where(x => x.userId == userId).ToList();
            if (elements.Count == 0)
                return element;
            element.scopeCode = elements.Average(x => x.scope_code);
            element.scopeDecor = elements.Average(x => x.scope_decor);

            element.scopeSecurity = elements.Average(x => x.scope_security);
            element.scopeMaintability = elements.Average(x => x.scope_maintability);
            element.scopeReability = elements.Average(x => x.scope_reability);
            element.scopeTeam = elements.Where(x => x.scope_team >0).Average(x => x.scope_team);
            Dictionary<string, int> languages = new Dictionary<string, int>();
            foreach (var item in elements)
            {
                if (item.language == null || item.language.Length == 0)
                {
                    if (!languages.ContainsKey("other"))
                        languages.Add("other", 1);
                    else
                        languages["other"]++;
                    continue;
                }
                if (!languages.ContainsKey(item.language))
                    languages.Add(item.language, 1);
                else
                    languages[item.language]++;
            }
            element.languageCounts = languages;
            return element;
        }
        public CompareUserRepoInfo GetAverageAllUsers()
        {
            using var context = new DataBaseConnection();
            CompareUserRepoInfo element = new CompareUserRepoInfo();
            List<Repo>? elements = context.Repos.ToList();
            if (elements.Count == 0)
                return element;
            element.scopeCode = elements.Average(x => x.scope_code);
            element.scopeDecor = elements.Average(x => x.scope_decor);

            element.scopeSecurity = elements.Average(x => x.scope_security);
            element.scopeMaintability = elements.Average(x => x.scope_maintability);
            element.scopeReability = elements.Average(x => x.scope_reability);
            element.scopeTeam = elements.Where(x => x.scope_team > 0).Average(x => x.scope_team);
            Dictionary<string, int> languages = new Dictionary<string, int>();
            foreach (var item in elements)
            {
                if (item.language == null || item.language.Length == 0)
                {
                    if (!languages.ContainsKey("other"))
                        languages.Add("other", 1);
                    else
                        languages["other"]++;
                    continue;
                }
                if (!languages.ContainsKey(item.language))
                    languages.Add(item.language, 1);
                else
                    languages[item.language]++;
            }
            element.languageCounts = languages;
            return element;
        }

        public void DeleteAll(long id)
        {
            using var context = new DataBaseConnection();
            var elements = context.Repos
                .Include(x => x.images)
                .Where(rec => rec.userId == id).ToList();
            foreach (var element in elements)
            {
                if (element.preview != null)
                    File.Delete(element.preview);
                if (element.images.Count > 0)
                    imageStorage.Delete(context, element.images);
            }

            context.Repos.RemoveRange(elements);
            context.SaveChanges();
        }
        public void DeleteByService(long userId, long serviceId)
        {
            using var context = new DataBaseConnection();
            var elements = context.Repos
                .Include(x => x.images)
                .Where(rec => rec.userId == userId && rec.serviceId == serviceId);

            foreach (var element in elements)
            {
                if (element.preview != null)
                    File.Delete(element.preview);
                if (element.images.Count > 0)
                    imageStorage.Delete(context, element.images);
            }

            context.Repos.RemoveRange(elements);
            context.SaveChanges();
        }
        public bool Delete(long id)
        {
            using var context = new DataBaseConnection();
            var element = context.Repos
                .Include(x => x.images)
                .FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                if (element.preview != null)
                    File.Delete(element.preview);
                if (element.images.Count > 0)
                    imageStorage.Delete(context, element.images);

                context.Repos.Remove(element);
                context.SaveChanges();
                return true;
            }
            throw new NullReferenceException("Не найден репозиторий с заданным id");
        }
        public async Task<bool> Update(RepoBindingModel model)
        {
            using var context = new DataBaseConnection();

            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Repos
                    .Include(x => x.images)
                    .Include(x => x.user)
                    .FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                    throw new NullReferenceException();

                element.title = model.title;
                element.description = model.description;
                element.link = model.link;
                element.language = model.language;

                string path = fileSaver.prepareDictionary(NAME, element.user.Id);
                if (model.preview != null)
                {
                    if (element.preview == null)
                    {
                        string file_path = await fileSaver.savePreview(path, model.preview);
                        element.preview = file_path;
                    }
                    else
                        await File.WriteAllBytesAsync(@$"{element.preview}", model.preview);
                }
                if (model.images == null)
                {
                    imageStorage.Delete(context, element.images);
                }
                else
                {
                    List<Image> deleteImage = element.images
                        .Where(x => !model.images.Any(y => y.id == x.Id)).ToList();
                    imageStorage.Delete(context, deleteImage);

                    List<byte[]> newImages = model.images
                        .Where(x => x.id == -1)
                        .Select(x => x.image)
                        .ToList();
                    await imageStorage.Create(context, newImages, path, NAME, element.Id);
                }
                context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch(NullReferenceException ex)
            {
                transaction.Rollback();
                throw new NullReferenceException("Не найден репозиторий с заданным id");
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Ошибка обновления репозитория");
            }

                
        }

        public async Task<RepoViewModel?> Get(long id)
        {
            using var context = new DataBaseConnection();
            Repo? element = context.Repos.Include(x => x.images).FirstOrDefault(x => x.Id == id);
            return element == null ? null : await element.GetViewModel();
        }

        public long GetUser(long id)
        {
            using var context = new DataBaseConnection();
            Repo? element = context.Repos.Include(x => x.images).FirstOrDefault(x => x.Id == id);
            return element.userId;
        }

        public async Task<List<RepoViewModel>> GetList(long id)
        {
            using var context = new DataBaseConnection();
            List<RepoViewModel> elements = new List<RepoViewModel>();
            foreach (var element in 
                context.Repos.Include(x => x.images)
                .Where(x => x.userId == id).OrderByDescending(x => x.date))
            {
                elements.Add(await element.GetViewModel());
            }
            return elements;
        }
    }
}
