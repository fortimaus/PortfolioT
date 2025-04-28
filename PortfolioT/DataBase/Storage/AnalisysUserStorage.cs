using Microsoft.AspNetCore.Mvc.ModelBinding;
using PortfolioT.DataBase.Models;
using PortfolioT.DataContracts.BindingModels;
using PortfolioT.DataContracts.StorageContracts;

namespace PortfolioT.DataBase.Storage
{
    public class AnalisysUserStorage : IAnalisysUserStorage
    {

        public AnalisisUser TryGet(AnalisysUserBindingModel model)
        {
            using var context = new DataBaseConnection();
            AnalisisUser? user = context.AnalisysUsers
                .FirstOrDefault(x => x.name.ToLower().Equals(model.name.ToLower()) && x.serviceId == model.serviceId);

            if(user == null)
            {
                Service? service = context.Services.FirstOrDefault(x => x.Id == model.serviceId);
                if (service == null)
                    return null;
                AnalisisUser NewUser = new AnalisisUser()
                {
                    name = model.name,
                    link = model.link,
                    service = service
                };
                user = context.AnalisysUsers.Add(NewUser).Entity;
                context.SaveChanges();
            }
  
            return user;
        }
        
    }
}
