using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Model
{
    internal class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }
        public string? Privileges { get; set; }

        public User()
        {
            Login = "";
            Password = "";
            Name = "";
            BirthDate = DateTime.Now;
            Address = "";
            Email = "";
            Phone = "";
            IsAdmin = false;
            Privileges = null;
        }
        public User(string login, string password, string name, DateTime birthDate, string address, string email, string phone, bool isAdmin, string? privileges)
        {
            Login = login;
            Password = password;
            Name = name;
            BirthDate = birthDate;
            Address = address;
            Email = email;
            Phone = phone;
            IsAdmin = isAdmin;
            Privileges = privileges;
        }

        public override string ToString()
        {
            return $"{Name} ({Login})";
        }
    }
}