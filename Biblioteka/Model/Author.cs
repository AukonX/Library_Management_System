using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Model
{
    internal class Author
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string Country {  get; set; }

        public Author()
        {
            Id = null;
            Name = "";
            BirthDate = DateTime.Parse("01.01.1970 00:00");
            DeathDate = null;
            Country = "";
        }
        public Author(int id, string name, DateTime birthDate, DateTime? deathDate, string country)
        {
            Id = id;
            Name = name;
            BirthDate = birthDate;
            DeathDate = deathDate;
            Country = country;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
