using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationData
{
    [Serializable]
    public class Authorization
    {
        public string name { get; set; }
        public string password { get; set; }
        public Authorization(string name, string password)
        {
            this.name = name;
            this.password = password;
        }
    }
}
