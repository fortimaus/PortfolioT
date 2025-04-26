using PortfolioT.DataBase.Models;
using System.IO;
using System.Xml.Linq;

namespace PortfolioT.DataBase.Storage
{
    public class FileSaver
    {
        private readonly string PATH = @"C:\test_zips";
        public string prepareDictionary(string name, long userId)
        {
            string path = @$"{PATH}\{name}\{userId}";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            return path;
        }
        public string prepareUsersDict()
        {
            string path = @$"{PATH}\users";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            return path;
        }
        private string randomName()
        {
            Random random = new Random();
            int[] nums = Enumerable.Range(0, 10).Select(v => random.Next(0, 10)).ToArray();
            return string.Join("", nums);
        }
        public async Task<string> savePreview(string path, byte[] data)
        {
            string image_name = $@"preview_{randomName()}.png";
            while (File.Exists(@$"{path}\{image_name}"))
                image_name = $@"preview_{randomName()}.png";
            await File.WriteAllBytesAsync(@$"{path}\{image_name}", data);
            return image_name;
        }
        public async Task<string> saveImage(string path,string name, long id, byte[] data)
        {
            string image_name = $@"{name}_{id}_{randomName()}.png";
            while (File.Exists(@$"{path}\{image_name}"))
                image_name = $@"{name}_{id}_{randomName()}.png";
            await File.WriteAllBytesAsync(@$"{path}\{image_name}", data);
            return @$"{path}\{image_name}";
        }
    }
}
