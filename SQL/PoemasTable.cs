using PoemaBotTelegram.Interface;
using MySql.Data.MySqlClient;
using System.Data.Common;
using PoemaBotTelegram.DataProvider;
using PoemaBotTelegram.LoggerService;
using PoemaBotTelegram.TgBotCLient;

namespace PoemaBotTelegram.SQL
{
    internal class PoemasTable
    {
        private readonly MySqlConnection _connection;
        private readonly Logger _logger;
        internal readonly int _countPoemas;
        internal PoemasTable()
        {
            _connection = new MySqlConnection(Constants.connectionString);
            _logger = new Logger(new FileService());
            _countPoemas = GetCountPoemas();
        }

        /// <summary>
        /// Добавляет в таблицу Poemas запись
        /// </summary>
        /// <param name="poema"></param>
        /// <returns></returns>
        internal async Task AddPoemas(Poema poema)
        {
            try
            {
                _connection.Open();
                string req = "INSERT INTO newdb.poemas (author, title, text) VALUES (@author,@title,@text)";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@author", poema.author);
                command.Parameters.AddWithValue("@title", poema.title);
                command.Parameters.AddWithValue("@text", poema.text);
                await command.ExecuteNonQueryAsync(); //записали
                _connection.Close();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to set data from the database", exception);
                throw;
            }
        }

        /// <summary>
        /// Получаем из таблицы Poemas одну запись
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Poema> GetRandomPoema(int id)
        {
            Poema result = new Poema { };
            try
            {
                _connection.Open();
                string req = "SELECT * FROM newdb.poemas WHERE id =@id LIMIT 1";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@id", id);
                DbDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows) while (reader.Read())
                    {
                        result.author = reader.GetString(1);
                        result.title = reader.GetString(2);
                        result.text = reader.GetString(3);
                    }
                _connection.Close();

            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to get data from the database", exception);
            }
            return result;
        }

        private int GetCountPoemas()
        {
            int result = 0;
            try
            {
                _connection.Open();
                string req = "SELECT COUNT(*) FROM newdb.poemas";
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
    }
}
