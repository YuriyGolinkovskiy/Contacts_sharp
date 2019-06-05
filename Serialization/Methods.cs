using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
   
    [Serializable]
    public class Methods
    {
        public List<List<object>> list { get; set; }
        public List<object> userData { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string phone { get; set; }
        public string dateOfBirth { get; set; }
        public bool status { get; set; }
        public Command com { get; set; }
        public Methods (Command com)
        {
            this.com = com;
            this.status = false;
        }
        public void Auth(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
        public void Registration(string login, string email, string name, string surname, string phone, string dateOfBirth, string password)
        {
            this.login = login;
            this.email = email;
            this.name = name;
            this.surname = surname;
            this.phone = phone;
            this.dateOfBirth = dateOfBirth;
            this.password = password;
        }
        public List<List<object>> GetUsersList()
        {
            return list;
        }
        public List<object> UserData()
        {
            return userData;
        }
    }
}
