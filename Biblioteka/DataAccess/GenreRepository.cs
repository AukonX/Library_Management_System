using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.DataAccess
{
    internal class GenreRepository
    {
        public static List<Model.Genre> GetAllGenres()
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            // Get all genres:
            MySqlCommand command = new MySqlCommand("SELECT * FROM genre", connection);
            var reader = command.ExecuteReader();

            List<Model.Genre> genres = new List<Model.Genre>();
            while (reader.Read())
            {
                genres.Add(
                    new Model.Genre(
                        int.Parse(reader["id"].ToString()),
                        reader["name"].ToString(),
                        reader["description"].ToString()
                    )
                );
            }

            connection.Close();
            return genres;
        }

        public static void SaveGenres(List<Model.Genre> genres)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            foreach (Model.Genre genre in genres)
            {
                if (genre.Id != null)
                {
                    MySqlCommand command = new MySqlCommand($"UPDATE genre SET name = '{genre.Name}', description = '{genre.Description}' " +
                        $"WHERE id = {genre.Id}", connection);
                    command.ExecuteNonQuery();
                }

                else
                {
                    MySqlCommand command = new MySqlCommand($"INSERT genre VALUE (null, '{genre.Name}', '{genre.Description}')", connection);
                    command.ExecuteNonQuery();
                }
            }

            connection.Close();
        }

        public static bool DeleteGenre(Model.Genre genre)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            bool status = true;
            try
            {
                MySqlCommand command = new MySqlCommand($"DELETE FROM genre WHERE id = {genre.Id}", connection);
                command.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                status = false;
            }


            connection.Close();

            return status;
        }

        public static Model.Genre GetBookGenres(string isbn)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"SELECT * FROM genre g, bookgenre bg WHERE g.id = bg.genre_id AND bg.isbn = \"{isbn}\"", connection);
            var reader = command.ExecuteReader();

            Model.Genre genre = null;
            while (reader.Read())
            {
                genre = new Model.Genre(
                    int.Parse(reader["id"].ToString()),
                    reader["name"].ToString(),
                    reader["description"].ToString()
                );
            }

            connection.Close();
            return genre;
        }
    }
}
