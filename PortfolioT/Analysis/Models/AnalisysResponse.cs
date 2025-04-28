using Microsoft.AspNetCore.Mvc;

namespace PortfolioT.Analysis.Models
{
    public class AnalisysResponse
    {
        public int id { get; set; }
        public float scope_cof { get; set; } = 0;

        public float scope_security { get; set; } = 5;
        public float scope_maintability { get; set; } = 5;
        public float scope_reability { get; set; } = 5;
        public string comments { get; set; } = string.Empty;

        public AnalisysResponse(int id)
        {
            this.id = id;
        }

        public AnalisysResponse(int id, float scope_cof, float security, float maintability, float reability)
        {
            this.id = id;
            this.scope_cof = scope_cof;
            this.scope_security = security;
            this.scope_maintability = maintability;
            this.scope_reability = reability;
        }
    }
}
