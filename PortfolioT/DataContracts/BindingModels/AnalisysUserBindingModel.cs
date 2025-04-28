using PortfolioT.DataBase.Models;
using PortfolioT.DataModels.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioT.DataContracts.BindingModels
{
    public class AnalisysUserBindingModel : IAnalisisUser
    {
        public long id { get; set; }
        public string name { get; set; } = string.Empty;

        public string link { get; set; } = string.Empty;
        public long serviceId { get; set; }
    }
}
