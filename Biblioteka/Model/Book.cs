using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Model
{
    internal class Book
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ReleaseDateString
        {
            get
            {
                return ReleaseDate.ToShortDateString();
            }
        }
        public int Pages { get; set; }

        public Book(string isbn, string title, string description, DateTime releaseDate, int pages)
        {
            Isbn = isbn;
            Title = title;
            Description = description;
            ReleaseDate = releaseDate;
            Pages = pages;
        }

        public override string ToString()
        {
            return $"{Title} ({Isbn})";
        }
    }
}
