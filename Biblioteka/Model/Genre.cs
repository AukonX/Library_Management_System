using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Model
{
    internal class Genre
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Genre()
        {
            Id = null;
            Name = "";
            Description = "";
        }
        public Genre(int? id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
