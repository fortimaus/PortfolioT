using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.BusinessLogicsContracts;
using PortfolioT.DataContracts.StorageContracts;
using System.IO;
using System.Xml.Linq;

namespace PortfolioT.DataBase.Storage
{
    public class ImageStorage : IImageStorage
    {
        public async Task<bool> Create(DataBaseConnection context, List<byte[]> images, string path, string name, long id)
        {
            FileSaver fileSaver = new FileSaver();

            List<Image> res_images = new List<Image>();

            Achievement achievement = context.Achievements.First(x => x.Id == id);

            if (achievement == null)
                throw new NullReferenceException("Не найдено достижение с заданным id");

            foreach (var image in images)
            {
                string path_image = await fileSaver.saveImage(path, name, id, image);
                Image newImage = new Image()
                {
                    achievement = achievement,
                    path = path_image
                };
                res_images.Add(newImage);
            }
            context.Images.AddRange(res_images);
            context.SaveChanges();
            return true;
        }

        public bool Delete(DataBaseConnection context, List<Image> images)
        {
            context.Images.RemoveRange(images);
            
            foreach (var image in images)
                File.Delete(image.path);

            
            context.SaveChanges();
            return true;
        }
    }
}
