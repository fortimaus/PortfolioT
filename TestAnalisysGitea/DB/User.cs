using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestS.DB
{
    class User
    {
        public long Id { get; set; }
        public string name { get; set; } = string.Empty;

        public int? count_repo { get; set; }

        public long? time { get; set; }
    }
}
