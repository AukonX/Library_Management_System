using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.Model
{
    internal class HistoricalBookInstance
    {
        public int Id { get; set; }
        public int BookInstanceId { get; set; }
        public string Login { get; set; }
        public DateOnly BorrowDate { get; set; }
        public DateOnly ReturnDate { get; set; }

        public HistoricalBookInstance(int id, int bookInstanceId, string login, DateOnly borrowDate, DateOnly returnDate)
        {
            Id = id;
            BookInstanceId = bookInstanceId;
            Login = login;
            BorrowDate = borrowDate;
            ReturnDate = returnDate;
        }
    }
}
