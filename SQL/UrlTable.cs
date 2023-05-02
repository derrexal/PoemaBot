using PoemaBotTelegram.Interface;
using MySql.Data.MySqlClient;
using System.Data.Common;
using PoemaBotTelegram.DataProvider;
using PoemaBotTelegram.LoggerService;



namespace PoemaBotTelegram.SQL
{
    internal class UrlTable
    {
        private readonly MySqlConnection _connection;
        private readonly Logger _logger;
        internal readonly int _countUrl;

        internal UrlTable()
        {
            _connection = new MySqlConnection(Constants.connectionString);
            _logger = new Logger(new FileService());
            _countUrl = GetCountUrl();
        }

        /// <summary>
        /// Добавляет в таблицу URL 1 запись
        /// </summary>
        /// <param name="poema"></param>
        /// <returns></returns>
        internal async Task AddUrl(Url url)
        {
            try
            {
                _connection.Open();
                string req = "INSERT INTO newdb.url (url, status) VALUES (@url, @status)";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@url", url.url);
                command.Parameters.AddWithValue("@status", url.status);
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
        /// Получаем из таблицы URL одну запись
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal async Task<Url> GetOneUrl(int id)
        {
            Url result = new Url { };
            try
            {
                _connection.Open();
                string req = "SELECT * FROM newdb.url WHERE id =@id LIMIT 1";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@id", id);
                DbDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows) while (reader.Read())
                    {
                        result.url = reader.GetString(1);
                        result.status = reader.GetString(2);
                    }
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
        /// Устанавливает в поле status таблицы URL значение - успешно 
        /// </summary>
        /// <param name="poema"></param>
        /// <returns></returns>
        internal async Task UpdateStatusUrl(int id)
        {
            try
            {
                _connection.Open();
                string req = "UPDATE newdb.url SET status=@status WHERE id=@id";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@status", Constants.statusSuccess);
                await command.ExecuteNonQueryAsync(); //записали
                _connection.Close();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to set status from the database", exception);
                throw;
            }
        }

        private int GetCountUrl()
        {
            int result=0;
            try
            {
                _connection.Open();
                string req = "SELECT COUNT(*) FROM newdb.url";
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

        internal async Task<int> GetNumberUrl()
        {
            int result = 0;
            string status = Constants.statusNone;
            try
            {
                _connection.Open();
                string req = "SELECT id FROM newdb.url Where status = @status LIMIT 1";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@status", status);
                DbDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows) while (reader.Read()) result = reader.GetInt32(0);
                _connection.Close();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to get NUMBERurl from the database", exception);
                throw;
            }
            return result;
        }
        
    }
}