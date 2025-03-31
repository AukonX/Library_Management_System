using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.DataAccess
{
    internal class ReviewRepository
    {
        public static List<Model.Review> GetAllReviews()
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            // Get all books:
            MySqlCommand command = new MySqlCommand("SELECT * FROM review", connection);
            var reader = command.ExecuteReader();

            List<Model.Review> reviews = new List<Model.Review>();
            while (reader.Read())
            {
                reviews.Add(
                    new Model.Review(
                        int.Parse(reader["id"].ToString()),
                        int.Parse(reader["rating"].ToString()),
                        reader["title"].ToString(),
                        reader["content"].ToString(),
                        DateTime.Parse(reader["time"].ToString()),
                        reader["isbn"].ToString(),
                        reader["login"].ToString()
                        
                    )
                );
            }

            connection.Close();
            return reviews;
        }

        public static string? AddReview(Model.Review review)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand($"INSERT review VALUE (null, {review.Rating}, \"{review.Title}\", \"{review.Content}\", STR_TO_DATE(\"{review.Time}\", \"%d.%m.%Y %H:%i:%s\"), \"{review.Isbn}\", \"{review.Login}\")", connection);
            command.ExecuteNonQuery();
            connection.Close();
            return null;
        }

        public static List<Model.Review> GetReviews(string isbn)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            // Get all books:
            MySqlCommand command = new MySqlCommand($"SELECT * FROM review WHERE isbn = \"{isbn}\"", connection);
            var reader = command.ExecuteReader();

            List<Model.Review> reviews = new List<Model.Review>();
            while (reader.Read())
            {
                reviews.Add(
                    new Model.Review(
                        int.Parse(reader["id"].ToString()),
                        int.Parse(reader["rating"].ToString()),
                        reader["title"].ToString(),
                        reader["content"].ToString(),
                        DateTime.Parse(reader["time"].ToString()),
                        reader["isbn"].ToString(),
                        reader["login"].ToString()

                    )
                );
            }

            connection.Close();
            return reviews;
        }
    }
}
