using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Model
{
    internal class FullBookInstance
    {
        public int? Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int MaxBorrowDays { get; set; }

        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int Pages { get; set; }

        public string? Login { get; set; }
        public DateTime? BorrowDate { get; set; }
        public string BorrowDateString
        {
            get
            {
                return BorrowDate?.ToShortDateString();
            }
        }

        public FullBookInstance(int? id, DateTime purchaseDate, int maxBorrowDays, string isbn, string title, string description, DateOnly releaseDate, int pages, string? login, DateTime? borrowDate)
        {
            Id = id;
            PurchaseDate = purchaseDate;
            MaxBorrowDays = maxBorrowDays;
            Isbn = isbn;
            Title = title;
            Description = description;
            ReleaseDate = releaseDate;
            Pages = pages;
            Login = login;
            BorrowDate = borrowDate;
        }
    }
}
