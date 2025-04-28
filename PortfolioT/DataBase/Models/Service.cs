using PortfolioT.DataContracts.ViewModels;
using PortfolioT.DataModels.Enums;
using PortfolioT.DataModels.Models;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PortfolioT.DataBase.Models
{
    public class Service : IService
    {
        [Required]
        public string title { get; set; } = string.Empty;

        public long Id { get; set; }

        public TypeService type { get; set; } = TypeService.None;

        public virtual List<UserService> userServices { get; set; } = new();

        public ServiceViewModel GetViewModel()
        {
            return new ServiceViewModel
            {
                title = title,
                Id = Id
            };
        }
    }
}
