using Biblioteka.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.DataAccess
{
    internal class AuthorRepository
    {
        public static List<Model.Author> GetAllAuthors()
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            // Get all authors:
            MySqlCommand command = new MySqlCommand("SELECT * FROM author", connection);
            var reader = command.ExecuteReader();

            List<Model.Author> authors = new List<Model.Author>();
            while (reader.Read())
            {
                authors.Add(
                    new Model.Author(
                        int.Parse(reader["id"].ToString()),
                        reader["name"].ToString(),
                        DateTime.Parse(reader["birth_date"].ToString()),
                        reader["death_date"].ToString() == "" ? null : DateTime.Parse(reader["death_date"].ToString()),
                        reader["country"].ToString()
                    )
                );
            }

            connection.Close();
            return authors;
        }

        public static void SaveAuthors(List<Model.Author> authors)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            foreach (Model.Author author in authors)
            {
                if (author.Id != null)
                {
                    MySqlCommand command = new MySqlCommand($"UPDATE author SET name = '{author.Name}', birth_date = STR_TO_DATE('{author.BirthDate.ToShortDateString()}', '%d.%m.%Y'), " +
                        $"death_date = IF('{author.DeathDate?.ToShortDateString()}' = '', null, STR_TO_DATE('{author.DeathDate?.ToShortDateString()}', '%d.%m.%Y')), " +
                        $"country = '{author.Country}' " +
                        $"WHERE id = {author.Id}", connection);
                    command.ExecuteNonQuery();
                }

                else
                {
                    MySqlCommand command = new MySqlCommand($"INSERT author VALUE (null, '{author.Name}', STR_TO_DATE('{author.BirthDate.ToShortDateString()}', '%d.%m.%Y'), " +
                        $"IF('{author.DeathDate?.ToShortDateString()}' = '', null, STR_TO_DATE('{author.DeathDate?.ToShortDateString()}', '%d.%m.%Y')), " +
                        $"'{author.Country}')", connection);
                    command.ExecuteNonQuery();
                }
            }

            connection.Close();
        }

        public static bool DeleteAuthor(Model.Author author)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            bool status = true;
            try
            {
                MySqlCommand command = new MySqlCommand($"DELETE FROM author WHERE id = {author.Id}", connection);
                command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                status = false;
            }
            

            connection.Close();

            return status;
        }

        public static List<string> GetAuthorsNatiotalities()
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

    
            MySqlCommand command = new MySqlCommand("SELECT DISTINCT country FROM author", connection);
            var reader = command.ExecuteReader();

            List<string> natiotalities = new List<string>();
            while (reader.Read())
            {
                natiotalities.Add(reader["country"].ToString());
            }
    
            connection.Close();
            return natiotalities;
        }

        public static Model.Author GetBookAuthor(string isbn)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"SELECT * FROM author a, bookauthor ba WHERE a.id = ba.author_id AND ba.isbn = \"{isbn}\"", connection);
            var reader = command.ExecuteReader();

            Model.Author author = null;
            while (reader.Read())
            {
                author = new Model.Author(
                    int.Parse(reader["id"].ToString()),
                    reader["name"].ToString(),
                    DateTime.Parse(reader["birth_date"].ToString()),
                    reader["death_date"].ToString() == "" ? null : DateTime.Parse(reader["death_date"].ToString()),
                    reader["country"].ToString()
                );
            }
            return author;
        }
    }
}
