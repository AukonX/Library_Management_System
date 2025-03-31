using Biblioteka.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.DataAccess
{
    internal class BookRepository
    {
        public static List<Model.Book> GetAllBooks()
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            // Get all books:
            MySqlCommand command = new MySqlCommand("SELECT * FROM book", connection);
            var reader = command.ExecuteReader();

            List<Model.Book> books = new List<Model.Book>();
            while (reader.Read())
            {
                books.Add(
                    new Model.Book(
                        reader["isbn"].ToString(),
                        reader["title"].ToString(),
                        reader["description"].ToString(),
                        DateTime.Parse(reader["release_date"].ToString()),
                        int.Parse(reader["pages"].ToString())
                    )
                );
            }

            connection.Close();
            return books;
        }

        public static string? AddBook(Model.Book book)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"INSERT book VALUE (\"{book.Isbn}\", \"{book.Title}\", \"{book.Description}\", STR_TO_DATE('{book.ReleaseDate.ToShortDateString()}', '%d.%m.%Y'), {book.Pages})", connection);
            if (book.Pages > int.MaxValue)
            {
                return "Za duża ilość stron";
            }
            command.ExecuteNonQuery();
            connection.Close();

            return null;
        }

        public static string? AddBookAuthor(string isbn, int id)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"INSERT bookauthor VALUE (\"{isbn}\", {id})", connection);
            command.ExecuteNonQuery();
            connection.Close();

            return null;
        }

        public static string? AddBookGenre(string isbn, int id)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"INSERT bookgenre VALUE (\"{isbn}\", {id})", connection);
            command.ExecuteNonQuery();
            connection.Close();

            return null;
        }
        
        public static string? ModifyBook(Model.Book book)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"UPDATE book set title = \"{book.Title}\", description = \"{book.Description}\", release_date = STR_TO_DATE('{book.ReleaseDate.ToShortDateString()}', '%d.%m.%Y'),pages = {book.Pages} WHERE isbn = \"{book.Isbn}\"", connection);
            command.ExecuteNonQuery();
            connection.Close();

            return null;
        }

        public static string? UpdateBookAuthor(string isbn, int id)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"UPDATE bookauthor SET author_id =  {id} WHERE isbn = \"{isbn}\"", connection);
            command.ExecuteNonQuery();
            connection.Close();

            return null;
        }

        public static string? UpdateBookGenre(string isbn, int id)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"UPDATE bookgenre SET genre_id = {id} WHERE isbn = \"{isbn}\"", connection);
            command.ExecuteNonQuery();
            connection.Close();

            return null;
        }

        public static string? DeleteBooks(string isbn)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"DELETE FROM bookgenre WHERE isbn = \"{isbn}\"", connection);
            command.ExecuteNonQuery();
            command = new MySqlCommand($"DELETE FROM bookauthor WHERE isbn = \"{isbn}\"", connection);
            command.ExecuteNonQuery();
            command = new MySqlCommand($"DELETE FROM book WHERE isbn = \"{isbn}\"", connection);
            command.ExecuteNonQuery();
            connection.Close();

            return null;
        }
    }
}