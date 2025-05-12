using Microsoft.EntityFrameworkCore;
using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.StorageContracts;
using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Models;
using System.IO;
using System.Xml.Linq;

namespace PortfolioT.DataBase.Storage
{
    public class AchievementStorage : IAchievementStorage
    {
        private readonly string NAME = "Achievement";

        private FileSaver fileSaver;
        private ImageStorage imageStorage;
        public AchievementStorage()
        {
            fileSaver = new FileSaver();
            imageStorage = new ImageStorage();
        }
        public async Task<bool> Create(AchievementBindingModel model)
        {
            using var context = new DataBaseConnection();
            User? user = context.Users.FirstOrDefault(x => x.Id == model.userId);
            
            if (user == null)
                throw new NullReferenceException("Не найден пользователь с заданным id");
            
            Achievement newElement = new Achievement()
            {
                title = model.title,
                description = model.description,
                link = model.link,
                user = user,
                service = null
            };

            string path = fileSaver.prepareDictionary(NAME, user.Id);

            if (model.preview == null)
                newElement.preview = null;
            else
            {
                string file_path = await fileSaver.savePreview(path, model.preview);
                newElement.preview = @$"{file_path}";
            }
            var element = context.Achievements.Add(newElement);
            context.SaveChanges();
            if (model.images != null)
            {
                await imageStorage.Create(context, model.images.Select(x => x.image).ToList(), path, NAME, element.Entity.Id);
            }
            return true;
        }

        public bool Delete(long id)
        {
            using var context = new DataBaseConnection();
            var element = context.Achievements
                .Include(x => x.images)
                .FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                if (element.preview != null)
                    File.Delete(element.preview);
                if (element.images.Count > 0)
                    imageStorage.Delete(context, element.images);

                context.Achievements.Remove(element);
                context.SaveChanges();
                return true;
            }
            throw new NullReferenceException("Не найдено достижение с заданным id");
        }
        public async Task<bool> Update(AchievementBindingModel model)
        {
            using var context = new DataBaseConnection();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Achievements
                    .Include(x => x.images)
                    .Include(x => x.user)
                    .FirstOrDefault(rec => rec.Id == model.Id);

                if (element == null)
                    throw new NullReferenceException();

                element.title = model.title;
                element.description = model.description;
                element.link = model.link;
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
                        .Where(x => !model.images.Any(y => y.id == x.Id)).ToList();
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
                throw new Exception("Ошибка обновления");
            }
            
        }

        public async Task<AchievementViewModel?> Get(long id)
        {
            using var context = new DataBaseConnection();
            Achievement? element = context.Achievements
                .Include(x => x.images).FirstOrDefault(x => x.Id == id);
            return element == null ? null : await element.GetViewModel();
        }

        public async Task<List<AchievementViewModel>> GetList(long id)
        {
            using var context = new DataBaseConnection();
            List<AchievementViewModel> elements = new List<AchievementViewModel>();
            var user_ach = context.Achievements.Include(x => x.images).Where(x => x.userId == id && x.basic);
            foreach (var element in user_ach)
            {
                elements.Add(await element.GetViewModel());
            }
            return elements;
        }

    }
}
