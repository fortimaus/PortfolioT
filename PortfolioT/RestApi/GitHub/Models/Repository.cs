using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioT.RestApi.GitHub.Models
{
    public class Repository
    {
        public int id { get; set; }

        public string name { get; set; } = string.Empty;

        public string full_name { get; set; } = string.Empty;

        public string updated_at { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public string language { get; set; } = string.Empty;

        public List<Branch> branches { get; set; } = new List<Branch>();

        public override string ToString()
        {
            return $"id: {id} Name: {name} FullName: {full_name} Date: {updated_at} desc: {description} language: {language}";
        }
    }
}
