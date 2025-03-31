using Biblioteka.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.DataAccess
{
    internal class BookDetialsRepository
    {
        public static List<Model.BookDetails> GetAllBookDetails()
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            // Get all books:
            MySqlCommand command = new MySqlCommand("SELECT b.title, b.description, b.isbn, b.release_date, b.pages, a.name, a.birth_date, a.death_date," +
                " a.country, g.name genre, g.description gendesc FROM book b, author a, genre g, bookauthor ba, bookgenre bg " +
                "WHERE  b.isbn = ba.isbn AND ba.author_id = a.id AND b.isbn = bg.isbn AND bg.genre_id = g.id", connection);
            var reader = command.ExecuteReader();

            List<Model.BookDetails> bookdetails = new List<Model.BookDetails>();
            while (reader.Read())
            {
                bookdetails.Add(
                    new Model.BookDetails(
                        reader["title"].ToString(),
                        reader["description"].ToString(),
                        reader["isbn"].ToString(),
                        DateOnly.FromDateTime(DateTime.Parse(reader["release_date"].ToString())),
                        int.Parse(reader["pages"].ToString()),
                        reader["name"].ToString(),
                        DateOnly.FromDateTime(DateTime.Parse(reader["birth_date"].ToString())),
                        reader["death_date"].ToString() == "" ? null : DateOnly.FromDateTime(DateTime.Parse(reader["death_date"].ToString())),
                        reader["country"].ToString(),
                        reader["genre"].ToString(),
                        reader["gendesc"].ToString()
                    )
                );
            }

            connection.Close();
            return bookdetails;
        }
    }
}
