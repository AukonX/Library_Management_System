using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Biblioteka.DataAccess
{
    internal class HistoricalBookInstanceDetailsRepository
    {
        public static List<Model.HistoricalBookInstanceDetails> GetUsersHistoricalBookInstances(Model.User user)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"SELECT b.title, a.name, b.isbn, g.name genre, h.borrow_date, h.return_date FROM history h, book b, " +
                $"bookinstance bi, author a, bookauthor ba, genre g, bookgenre bg WHERE h.login = \"{user.Login}\" AND h.bookinstance_id = bi.id AND bi.isbn = b.isbn AND " +
                $"ba.isbn = b.isbn AND ba.author_id = a.id AND bg.isbn = b.isbn AND bg.genre_id = g.id", connection);
            var reader = command.ExecuteReader();

            List<Model.HistoricalBookInstanceDetails> details = new List<Model.HistoricalBookInstanceDetails>();
            while (reader.Read())
            {
                details.Add(
                    new Model.HistoricalBookInstanceDetails(
                        reader["title"].ToString(),
                        reader["name"].ToString(),
                        reader["isbn"].ToString(),
                        reader["genre"].ToString(),
                        DateOnly.FromDateTime(DateTime.Parse(reader["borrow_date"].ToString())),
                        DateOnly.FromDateTime(DateTime.Parse(reader["return_date"].ToString()))
                        )
                    );
            }
            connection.Close();
            return details;
        }
    }
}
