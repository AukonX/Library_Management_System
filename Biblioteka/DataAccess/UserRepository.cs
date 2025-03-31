using Biblioteka.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.DataAccess
{
    internal class UserRepository
    {
        public static List<Model.User> GetAllUsers()
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand("SELECT user.login, user.password, user.name, user.birth_date, user.address, user.email, user.phone, IFNULL(isuseradmin.admin_id, 0) is_admin, administrator.privileges privileges " +
                "FROM user LEFT JOIN isuseradmin ON user.login = isuseradmin.login " +
                "LEFT JOIN administrator ON isuseradmin.admin_id = administrator.id", connection);
            var reader = command.ExecuteReader();

            List<Model.User> users = new List<Model.User>();
            while (reader.Read())
            {
                users.Add(
                    new Model.User(
                        reader["login"].ToString(),
                        reader["password"].ToString(),
                        reader["name"].ToString(),
                        DateTime.Parse(reader["birth_date"].ToString()),
                        reader["address"].ToString(),
                        reader["email"].ToString(),
                        reader["phone"].ToString(),
                        int.Parse(reader["is_admin"].ToString()) == 0 ? false : true,
                        reader["privileges"].ToString()
                    )    
                );
            }

            connection.Close();
            return users;
        }

        public static Model.User? GetUserByCredentials(string username, string password)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand("SELECT user.login, user.password, user.name, user.birth_date, user.address, user.email, user.phone, IFNULL(isuseradmin.admin_id, 0) is_admin, administrator.privileges privileges " +
                 "FROM user LEFT JOIN isuseradmin ON user.login = isuseradmin.login " +
                 "LEFT JOIN administrator ON isuseradmin.admin_id = administrator.id " +
                 $"WHERE user.login = '{username}' AND user.password = '{password}'", connection);
            var reader = command.ExecuteReader();

            reader.Read();

            Model.User? user = null;
            if (reader.HasRows)
            {
                user = new Model.User(
                    reader["login"].ToString(),
                    reader["password"].ToString(),
                    reader["name"].ToString(),
                    DateTime.Parse(reader["birth_date"].ToString()),
                    reader["address"].ToString(),
                    reader["email"].ToString(),
                    reader["phone"].ToString(),
                    int.Parse(reader["is_admin"].ToString()) == 0 ? false : true,
                    reader["privileges"].ToString()
                );
            }

            return user;
        }

        public static List<string> GetUsersWithOverdueBooks()
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command = new MySqlCommand("SELECT DISTINCT user.login, user.name FROM user, bookinstance " +
                "WHERE bookinstance.login = user.login AND abs(datediff(now(), bookinstance.borrow_date)) > bookinstance.max_borrow_days", connection);
            var reader = command.ExecuteReader();

            List<string> users = new List<string>();
            while (reader.Read())
            {
                users.Add(
                    $"{reader["name"]} ({reader["login"]})"
                );
            }

            connection.Close();
            return users;
        }

        public static void SaveUsers(List<Model.User> users)
        {
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command;
            foreach(var user in users)
            {
                command = new MySqlCommand($"SELECT * FROM user WHERE login = '{user.Login}'", connection);
                var reader1 = command.ExecuteReader();
                bool userExists = reader1.HasRows;
                reader1.Close();

                // If user with this login already exists perform UPDATE query:
                if (userExists)
                {
                    command = new MySqlCommand($"UPDATE user SET password = '{user.Password}', name = '{user.Name}', birth_date = STR_TO_DATE('{user.BirthDate.ToShortDateString()}', '%d.%m.%Y'), " +
                        $"address = '{user.Address}', email = '{user.Email}', phone = '{user.Phone}' " +
                        $"WHERE login = '{user.Login}'", connection);
                    command.ExecuteNonQuery();

                    // Edit admin privileges:
                    command = new MySqlCommand($"SELECT * FROM isuseradmin WHERE login = '{user.Login}'", connection);
                    var reader2 = command.ExecuteReader();
                    bool adminExists = reader2.HasRows;
                    reader2.Close();

                    // If user was an admin and is not longer an admin:
                    if (adminExists && !user.IsAdmin)
                    {
                        command = new MySqlCommand($"DELETE FROM isuseradmin WHERE login = '{user.Login}'", connection);
                        command.ExecuteNonQuery();
                    }

                    // If user wasn't an admin and now is an admin:
                    else if (!adminExists && user.IsAdmin)
                    {
                        command = new MySqlCommand($"INSERT isuseradmin VALUE ('{user.Login}', 1)", connection);
                        command.ExecuteNonQuery();
                    }
                }

                // If user with this login doesn't exist perform INSERT query:
                else
                {
                    command = new MySqlCommand($"INSERT user VALUE ('{user.Login}', '{user.Password}', '{user.Name}', STR_TO_DATE('{user.BirthDate.ToShortDateString()}', '%d.%m.%Y'), " +
                        $"'{user.Address}', '{user.Email}', '{user.Phone}')", connection);
                    command.ExecuteNonQuery();

                    // Grant admin privileges if user is admin:
                    if (user.IsAdmin)
                    {
                        command = new MySqlCommand($"INSERT isuseradmin VALUE ('{user.Login}', 1)", connection);
                        command.ExecuteNonQuery();
                    }
                }
            }

            connection.Close();
        }

        public static string DeleteUser(Model.User user)
        {
            string status = "";
            MySqlConnection connection = DBConnection.Instance.Connection;
            connection.Open();

            MySqlCommand command;
            MySqlDataReader reader;

            // Check if user exists in database:
            command = new MySqlCommand($"SELECT * FROM user WHERE login = '{user.Login}'", connection);
            reader = command.ExecuteReader();
            bool userExists = reader.HasRows;
            reader.Close();

            // Check if user has unreturned book instances:
            command = new MySqlCommand($"SELECT * FROM bookinstance WHERE login = '{user.Login}'", connection);
            reader = command.ExecuteReader();
            bool userHasUnreturnedBooksInstances = reader.HasRows;
            reader.Close();

            if (userExists)
            {
                if (!userHasUnreturnedBooksInstances)
                {
                    try
                    {
                        command = new MySqlCommand($"DELETE FROM isuseradmin WHERE login = '{user.Login}'", connection);
                        command.ExecuteNonQuery();

                        command = new MySqlCommand($"DELETE FROM history WHERE login = '{user.Login}'", connection);
                        command.ExecuteNonQuery();

                        command = new MySqlCommand($"DELETE FROM review WHERE login = '{user.Login}'", connection);
                        command.ExecuteNonQuery();

                        command = new MySqlCommand($"DELETE FROM user WHERE login = '{user.Login}'", connection);
                        command.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        status = "Nie można usunąć użytkownika, ponieważ odwołują się do niego niektóre obiekty!";
                    }
                }
                
                else
                {
                    status = "Nie można usunąć użytkownika, ponieważ ma on nieoddane książki!";
                }
            }

            else
            {
                status = "Użytkownik nie istnieje w bazie danych!";
            }

            connection.Close();
            return status;
        }
    }
}
