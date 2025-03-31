using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Model
{
    internal class HistoricalBookInstanceDetails
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public string Genre { get; set; }
        public DateOnly BorrowDate { get; set; }
        public DateOnly ReturnDate { get; set; }


        public HistoricalBookInstanceDetails(string title, string author, string isbn, string genre, DateOnly borrowdate, DateOnly returnDate)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
            Genre = genre;
            BorrowDate = borrowdate;
            ReturnDate = returnDate;
        }

        public override string ToString()
        {
            return $"{Title} || {Author} || {Isbn} || {Genre} || {BorrowDate.ToShortDateString()} || {ReturnDate.ToShortDateString()}";
        }
    }
}
