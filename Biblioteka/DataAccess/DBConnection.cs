using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka.DataAccess
{
    internal class DBConnection
    {
        private MySqlConnectionStringBuilder _builder = new MySqlConnectionStringBuilder();

        private static DBConnection _instance = null;
        public static DBConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DBConnection();
                }
                return _instance;
            }
        }

        public MySqlConnection Connection => new MySqlConnection(_builder.ToString());

        private DBConnection()
        {
            _builder.Server = ConfigurationManager.AppSettings["server"];
            _builder.Database = ConfigurationManager.AppSettings["database"];
            _builder.UserID = ConfigurationManager.AppSettings["user"];
            _builder.Password = ConfigurationManager.AppSettings["password"];
            _builder.Port = uint.Parse(ConfigurationManager.AppSettings["port"]);
        }
    }
}
