using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.DataAccess
{
    internal class LoanedBooksRepository
    {
        public static List<string> GetUserLoanedBookInstances(Model.User user)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"SELECT title, bi.max_borrow_days - datediff(now(), bi.borrow_date) pozostalo FROM bookinstance bi, book b WHERE bi.login = \"{user.Login}\" AND b.isbn = bi.isbn AND abs(datediff(now(), bi.borrow_date)) < bi.max_borrow_days", connection);
            var reader = command.ExecuteReader();

            List<string> loanedBooks = new List<string>();
            while (reader.Read())
            {
                loanedBooks.Add(
                    reader["title"].ToString() + " || Dni do oddania: " + reader["pozostalo"].ToString()
                );
            }

            connection.Close();
            return loanedBooks;
        }

        public static List<string> GetUserOverdueBooks(Model.User user)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"SELECT title, abs(datediff(now(), bi.borrow_date)) uplynelo FROM book b, bookinstance bi WHERE bi.login = \"{user.Login}\" AND abs(datediff(now(), bi.borrow_date)) > bi.max_borrow_days AND b.isbn = bi.isbn", connection);
            var reader = command.ExecuteReader();

            List<string> overdueBooks = new List<string>();
            while (reader.Read())
            {
                overdueBooks.Add(
                    reader["title"].ToString() + " || ilość dni po terminie: " + reader["uplynelo"].ToString()
                );
            }

            connection.Close();
            return overdueBooks;
        }
    }
}
