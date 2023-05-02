using PoemaBotTelegram.Interface;
using MySql.Data.MySqlClient;
using System.Data.Common;
using PoemaBotTelegram.DataProvider;
using PoemaBotTelegram.LoggerService;
using PoemaBotTelegram.TgBotCLient;
using Mysqlx.Crud;
using static System.Net.Mime.MediaTypeNames;


namespace PoemaBotTelegram.SQL
{
    internal class PoemasCategoryTable
    {
        private readonly MySqlConnection _connection;
        private readonly Logger _logger;
        private static int _countRecord;

        /// <summary>
        /// Возвращает количество записей в таблице на данный момент
        /// </summary>
        internal static int CountRecord
        {
            get { return _countRecord; }
            private set { _countRecord = value; }
        }

        internal PoemasCategoryTable()
        {
            _connection = new MySqlConnection(Constants.connectionString);
            _logger = new Logger(new FileService());
            CountRecord = GetCountRecord(); // получается, получаем записи перед самым обращением к таблице. 
        }

        /// <summary>
        /// Получаем количество записей в таблице "Стихи и категории"
        /// </summary>
        /// <returns></returns>
        private int GetCountRecord()
        {
            int result = 0;
            try
            {
                _connection.Open();
                string req = "SELECT COUNT(*) FROM newdb.poemas_category";
                MySqlCommand command = new MySqlCommand(req, _connection);
                DbDataReader reader = command.ExecuteReader();
                if (reader.HasRows) while (reader.Read()) result = reader.GetInt32(0);
                _connection.Close();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to get data from the database", exception);
                throw;
            }
            return result;
        }

        /// <summary>
        /// Добавляем новую запись в таблицу "Стихи и категории"
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        internal async Task AddRecord(int poemas_id, int category_id)
        {
            try
            {
                _connection.Open();
                string req = "INSERT INTO newdb.poemas_category (poemas_id,category_id) VALUES (@poemas_id,@category_id)";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@poemas_id", poemas_id);
                command.Parameters.AddWithValue("@category_id", category_id);
                await command.ExecuteNonQueryAsync(); //записали
                _connection.Close();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to set data from the database", exception);
                throw;
            }
        }
    }
}
