using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SQLite;
using Dapper;

namespace Auto_Twitter
{
    internal class SQLite_Data_Access
    {
        public static List<Command_model> LoadCommands()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Command_model>("SELECT * FROM Comms", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveCommand(Command_model command)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("INSERT INTO Comms(name, content, image, data) VALUES(@name, @content, @image, @data)", command);
            }
        }

        public static void DeleteCommand(string command)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute($"DELETE FROM Comms WHERE name = '{command}'");
            }
        }

        public static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
