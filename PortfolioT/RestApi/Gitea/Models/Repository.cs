using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.Gitea.Models
{
    public class Repository
    {
        public long id { get; set; }
        public string name { get; set; } = string.Empty;
        public string full_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public bool empty { get; set; }
        public int size { get; set; }
        public string language { get; set; } = string.Empty;
        public string updated_at { get; set; } = string.Empty;

        public List<Branch> branches { get; set; } = new List<Branch>();

        public override string ToString()
        {
            return $"id: {id} Name: {name} Full_name: {full_name} Desc: {description} Empty: {empty} Size: {size} Language: {language} Update_at: {updated_at}";
        }
    }
}
