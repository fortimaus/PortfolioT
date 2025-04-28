using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.ViewModels;

namespace PortfolioT.DataContracts.StorageContracts
{
    public interface IAnalisysUserStorage
    {
        AnalisisUser TryGet(AnalisysUserBindingModel model);
    }
}
