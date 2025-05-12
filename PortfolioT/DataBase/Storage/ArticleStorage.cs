using Microsoft.EntityFrameworkCore;
using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataBase.Storage
{
    public class ArticleStorage : IArticleStorage
    {
        
        private readonly string NAME = "Article";
        private FileSaver fileSaver;
        private ImageStorage imageStorage;
        public ArticleStorage()
        {
            fileSaver = new FileSaver();
            imageStorage = new ImageStorage();
        }
        public async Task<bool> Create(ArticleBindingModel model)
        {
            using var context = new DataBaseConnection();
            User? user = context.Users.FirstOrDefault(x => x.Id == model.userId);
            Service? service = context.Services.FirstOrDefault(x => x.Id == model.serviceId);
            if (user == null)
                throw new NullReferenceException("Не найден пользователь с заданным id");

            Article newArt = new Article()
            {
                title = model.title,
                description = model.description,
                link = model.link,
                user = user,
                words = model.words,
                service = service,
                basic = false
            };

            string path = fileSaver.prepareDictionary(NAME, user.Id);

            if (model.preview == null)
                newArt.preview = null;
            else
            {
                string file_name = await fileSaver.savePreview(path, model.preview);
                newArt.preview = @$"{path}\{file_name}";
            }
            var art = context.Articles.Add(newArt);
            context.SaveChanges();
            if(model.images != null)
            {
                await imageStorage.Create(context,model.images.Select(x => x.image).ToList(), path, NAME, art.Entity.Id);
            }
            return true;
        }

        public void DeleteAll(long id)
        {
            using var context = new DataBaseConnection();
            var elements = context.Articles
                .Include(x => x.images)
                .Where(rec => rec.userId == id).ToList();
            foreach(var element in elements)
            {
                if (element.preview != null)
                    File.Delete(element.preview);
                if (element.images.Count > 0)
                    imageStorage.Delete(context, element.images);
            }

            context.Articles.RemoveRange(elements);
            context.SaveChanges();
        }
        public void DeleteByService(long userId, long serviceId)
        {
            using var context = new DataBaseConnection();
            var elements = context.Articles
                .Include(x => x.images)
                .Where(rec => rec.userId == userId && rec.serviceId == serviceId);

            foreach (var element in elements)
            {
                if (element.preview != null)
                    File.Delete(element.preview);
                if (element.images.Count > 0)
                    imageStorage.Delete(context, element.images);
            }

            context.Articles.RemoveRange(elements);
            context.SaveChanges();
        }
        public bool Delete(long id)
        {
            using var context = new DataBaseConnection();
            var element = context.Articles
                .Include(x => x.images)
                .FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                if (element.preview != null)
                    File.Delete(element.preview);
                if (element.images.Count > 0)
                    imageStorage.Delete(context, element.images);

                context.Articles.Remove(element);
                context.SaveChanges();
                return true;
            }
            throw new NullReferenceException("Не найдена статья с заданным id");
        }
        public async Task<bool> Update(ArticleBindingModel model)
        {
            using var context = new DataBaseConnection();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Articles
                    .Include(x => x.images)
                    .Include(x => x.user)
                    .FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                    throw new NullReferenceException();

                element.title = model.title;
                element.description = model.description;
                element.link = model.link;
                element.words = model.words;


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
                context.SaveChanges();
                if (model.images == null)
                {
                    imageStorage.Delete(context, element.images);
                }
                else
                {
                    List<Image> deleteImage = element.images
                        .Where(x => !model.images.Any(y => y.id == x.Id)).ToList(); ;
                    imageStorage.Delete(context, deleteImage);

                    List<byte[]> newImages = model.images
                        .Where(x => x.id == -1)
                        .Select(x => x.image)
                        .ToList();
                    await imageStorage.Create(context, newImages, path, NAME, element.Id);
                }
                transaction.Commit();
                return true;
            }
            catch(NullReferenceException ex)
            {
                transaction.Rollback();
                throw new NullReferenceException("Не найдено достижение с заданным id");
            }catch(Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Ошибка обновления статьи");
            }
            
        }

        public async Task<ArticleViewModel?> Get(long id)
        {
            using var context = new DataBaseConnection();
            Article? article = context.Articles.Include(x => x.images).FirstOrDefault(x => x.Id == id);
            return article == null ? null : await article.GetViewModel();
        }

        public async Task<List<ArticleViewModel>> GetList(long id)
        {
            using var context = new DataBaseConnection();
            List<ArticleViewModel> repos = new List<ArticleViewModel>();
            foreach (var art in 
                context.Articles.Include(x => x.images).Where(x => x.userId == id))
            {
                repos.Add(await art.GetViewModel());
            }
            return repos;
        }
    }
}
