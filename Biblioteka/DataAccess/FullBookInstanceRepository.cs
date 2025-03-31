using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.DataAccess
{
    internal class FullBookInstanceRepository
    {
        public static List<Model.FullBookInstance> GetAllFullBooksInstances()
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            // Get all books instances:
            MySqlCommand command = new MySqlCommand("SELECT bi.id, bi.purchase_date, bi.max_borrow_days, b.isbn, b.title, b.description, b.release_date, b.pages, bi.login, bi.borrow_date " +
                "FROM bookinstance bi, book b WHERE bi.isbn = b.isbn", connection);
            var reader = command.ExecuteReader();

            List<Model.FullBookInstance> fullBooksinstances = new List<Model.FullBookInstance>();
            while (reader.Read())
            {
                fullBooksinstances.Add(
                    new Model.FullBookInstance(
                        int.Parse(reader["id"].ToString()),
                        DateTime.Parse(reader["purchase_date"].ToString()),
                        int.Parse(reader["max_borrow_days"].ToString()),
                        reader["isbn"].ToString(),
                        reader["title"].ToString(),
                        reader["description"].ToString(),
                        DateOnly.FromDateTime(DateTime.Parse(reader["release_date"].ToString())),
                        int.Parse(reader["pages"].ToString()),
                        reader["login"].ToString() == "" ? null : reader["login"].ToString(),
                        reader["borrow_date"].ToString() == "" ? null : DateTime.Parse(reader["borrow_date"].ToString())
                    )
                );
            }

            connection.Close();
            return fullBooksinstances;
        }
    }
}
