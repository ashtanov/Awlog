using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Awlog.Models
{
    public class BUser
    {
        public BUser()
        {
        }

        public BUser(int id, string login, string name, string surname, string email, string passwordHash)
        {
            Id = id;
            PasswordHash = passwordHash;
            Email = email;
            Name = name;
            Surname = surname;
            Login = login;
        }

        public int Id;
        public string PasswordHash;
        public string Login;
        public string Name;
        public string Surname;
        public string Email;
    }
}
