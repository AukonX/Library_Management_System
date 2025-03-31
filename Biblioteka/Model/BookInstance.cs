using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Model
{
    internal class BookInstance
    {
        public int? Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseDateString
        {
            get
            {
                return PurchaseDate.ToShortDateString();
            }
        }
        public int MaxBorrowDays { get; set; }
        public string Isbn { get; set; }
        public string? Login { get; set; }
        public DateTime? BorrowDate { get; set; }

        public BookInstance(int? id, DateTime purchaseDate, int maxBorrowDays, string isbn, string? login, DateTime? borrowDate)
        {
            Id = id;
            PurchaseDate = purchaseDate;
            MaxBorrowDays = maxBorrowDays;
            Isbn = isbn;
            Login = login;
            BorrowDate = borrowDate;
        }

        public override string ToString()
        {
            if (Id == null)
            {
                return $"[Nowa instancja książki]";
            }

            else
            {
                return $"ID: {Id} | Zakup: {PurchaseDate.ToShortDateString()}";
            }
        }
    }
}