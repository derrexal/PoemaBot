using MySql.Data.MySqlClient;

namespace PoemaBotTelegram.SQL
{
    internal class DB 
    {
        internal CategoryTable categoryTable;
        internal PoemasTable poemasTable;
        internal PoemasCategoryTable poemasCategoryTable;
        internal UrlTable urlTable;
        internal UsersTable usersTable;

        internal DB()
        {
<<<<<<< Updated upstream
            connectionString = GetConnectionString();
            MySqlConnection connection = new MySqlConnection(connectionString);

            categoryTable = new CategoryTable(connection);
            poemasTable = new PoemasTable(connection);
            poemasCategoryTable = new PoemasCategoryTable(connection);
            urlTable = new UrlTable(connection);
            usersTable = new UsersTable(connection);
=======
            categoryTable = new CategoryTable();
            poemasTable = new PoemasTable();
            poemasCategoryTable = new PoemasCategoryTable();
            urlTable = new UrlTable();
            usersTable = new UsersTable();
>>>>>>> Stashed changes
        }

        /// <summary>
        /// Формируем строку подключения из заданных параметров
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            MySqlConnectionStringBuilder csb = new MySqlConnectionStringBuilder
            {
                Server = Settings.ADDRESS,
                Port = Settings.PORT,
                UserID = Settings.DB_USERNAME,
                Password = Settings.DB_PASSWORD,
            };
            return csb.ConnectionString;
        }
    }
}
