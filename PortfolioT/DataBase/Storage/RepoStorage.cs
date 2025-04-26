using Microsoft.EntityFrameworkCore;
using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
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
            User? user = context.Users.First(x => x.Id == model.userId);
            if (user == null)
                return false;
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
                scope_security = model.scope_security

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
                await imageStorage.Create(context, model.images.Select(x => x.Item2).ToList(), path, NAME, element.Entity.Id);
            }
            return true;
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
            return false;
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
                    return false;

                element.title = model.title;
                element.description = model.description;
                element.link = model.link;
                element.language = model.language;

                string path = fileSaver.prepareDictionary(NAME, element.user.Id);
                if (model.preview != null)
                {
                    if (element.preview == null)
                    {
                        string file_name = await fileSaver.savePreview(path, model.preview);
                        element.preview = @$"{path}\{file_name}";
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
                        .Where(x => !model.images.Any(y => y.Item1 == x.Id)).ToList();
                    imageStorage.Delete(context, deleteImage);

                    List<byte[]> newImages = model.images
                        .Where(x => x.Item1 == -1)
                        .Select(x => x.Item2)
                        .ToList();
                    await imageStorage.Create(context, newImages, path, NAME, element.Id);
                }
                context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }

                
        }

        public async Task<RepoViewModel?> Get(long id)
        {
            using var context = new DataBaseConnection();
            Repo? element = context.Repos.Include(x => x.images).FirstOrDefault(x => x.Id == id);
            return element == null ? null : await element.GetViewModel();
        }

        public async Task<List<RepoViewModel>> GetList(long id)
        {
            using var context = new DataBaseConnection();
            List<RepoViewModel> elements = new List<RepoViewModel>();
            foreach (var element in 
                context.Repos.Include(x => x.images).Where(x => x.userId == id))
            {
                elements.Add(await element.GetViewModel());
            }
            return elements;
        }
    }
}
