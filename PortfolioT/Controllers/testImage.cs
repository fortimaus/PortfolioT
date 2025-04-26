namespace PortfolioT.Controllers
{
    public class testImage
    {
        public async Task<byte[]> preview()
        {
            byte[] preview = await File.ReadAllBytesAsync(@"C:\test_zips\dog.png");
            return preview;
        }

        public async Task<byte[]> image1()
        {
            byte[] preview = await File.ReadAllBytesAsync(@"C:\test_zips\human.jpg");
            return preview;
        }
        public async Task<byte[]> image2()
        {
            byte[] preview = await File.ReadAllBytesAsync(@"C:\test_zips\bone.jpg");
            return preview;
        }

    }
}
