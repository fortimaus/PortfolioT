using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.BusinessLogicsContracts
{
    public interface IImageStorage
    {
        List<ImageViewModel> GetList();
        ImageViewModel Get(long id);
        bool Create(ImageBindingModel model);
        bool Update(ImageBindingModel model);
        bool Delete(ImageBindingModel model);
    }
}
