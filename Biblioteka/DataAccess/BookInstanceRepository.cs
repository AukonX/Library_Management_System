using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.DataAccess
{
    internal class BookInstanceRepository
    {
        public static List<Model.BookInstance> GetAllBooksInstances()
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            // Get all books instances:
            MySqlCommand command = new MySqlCommand("SELECT * FROM bookinstance", connection);
            var reader = command.ExecuteReader();

            List<Model.BookInstance> booksinstances = new List<Model.BookInstance>();
            while (reader.Read())
            {
                booksinstances.Add(
                    new Model.BookInstance(
                        int.Parse(reader["id"].ToString()),
                        DateTime.Parse(reader["purchase_date"].ToString()),
                        int.Parse(reader["max_borrow_days"].ToString()),
                        reader["isbn"].ToString(),
                        reader["login"].ToString() == "" ? null : reader["login"].ToString(),
                        reader["borrow_date"].ToString() == "" ? null : DateTime.Parse(reader["borrow_date"].ToString())
                    )
                );
            }

            connection.Close();
            return booksinstances;
        }

        public static string? ModifyBookInstance(Model.BookInstance bookInstance)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command;
            // INSERT if Id is null:
            if (bookInstance.Id == null)
            {
                command = new MySqlCommand($"INSERT bookinstance VALUE (null, STR_TO_DATE('{bookInstance.PurchaseDate.ToShortDateString()}', '%d.%m.%Y'), {bookInstance.MaxBorrowDays}, '{bookInstance.Isbn}', null, null)", connection);
            }

            // UPDATE if Id is not null:
            else
            {
                command = new MySqlCommand($"UPDATE bookinstance SET purchase_date = STR_TO_DATE('{bookInstance.PurchaseDate.ToShortDateString()}', '%d.%m.%Y'), max_borrow_days = {bookInstance.MaxBorrowDays} WHERE id = {bookInstance.Id}", connection);
            }
            
            // Check if INT isn't overflowed:
            if (bookInstance.MaxBorrowDays > int.MaxValue)
            {
               return $"Wartość max czasu wypożyczenia może wynosić maksymalnie {int.MaxValue} dni";
            }
            
            command.ExecuteNonQuery();
            connection.Close();

            return null;
        }

        public static void DeleteBookInstance(Model.BookInstance bookInstance)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"DELETE FROM bookinstance WHERE id = {bookInstance.Id}", connection);

            command.ExecuteNonQuery();
            connection.Close();
        }

        public static void RemoveUserFromBookInstance(Model.BookInstance bookInstance)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command;
            // Remove book instance:
            command = new MySqlCommand($"UPDATE bookinstance SET login = null, borrow_date = null WHERE id = {bookInstance.Id}", connection);
            command.ExecuteNonQuery();

            // Add book instance to history:
            command = new MySqlCommand($"INSERT history VALUE (null, {bookInstance.Id}, '{bookInstance.Login}', " +
                $"STR_TO_DATE('{bookInstance.BorrowDate?.ToShortDateString()}', '%d.%m.%Y'), NOW())", connection);
            command.ExecuteNonQuery();

            connection.Close();
        }

        public static void LendBookInstance(Model.BookInstance bookInstance)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"UPDATE bookinstance SET login = '{bookInstance.Login}', borrow_date = NOW() WHERE id = {bookInstance.Id}", connection);

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}