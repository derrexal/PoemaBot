using PoemaBotTelegram.Interface;
using MySql.Data.MySqlClient;
using System.Data.Common;
using PoemaBotTelegram.LoggerService;
using PoemaBotTelegram.TgBotCLient;

namespace PoemaBotTelegram.SQL
{
    internal class UsersTable
    {
        private readonly MySqlConnection _connection;
        private readonly Logger _logger;
        internal readonly int _countUsers;

        internal UsersTable()
        {
            _connection = new MySqlConnection(Constants.connectionString);
            _logger = new Logger(new FileService());
            _countUsers = GetCountUsers();
        }

        private int GetCountUsers()
        {
            int result = 0;
            try
            {
                _connection.Open();
                string req = "SELECT COUNT(*) FROM newdb.users";
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
        /// Если такого юзьверя не добавляли - добавляем. Если есть такой ИД в таблице, но отличается username - обновляем никнейм
        /// </summary>
        /// <param name="id_tg"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task CheckAndAddUser(long id_tg, string? username)
        {
            using (_connection)
            { // можно упростить check через exists
                int id_record = await CheckID(id_tg); // номер записи в таблице
                if (id_record != 0)                     // Если в таблице есть такой юзьверь
                {
                    string username_record = await GetUsernameFromIdRecord(id_record);
                    if (username_record == username) return;                                      // Если нашли запись с таким юзьверем - выходим
                    await UpdateUsername(id_record, username);                             // Если нашли запись с таким ID но другим username - обновялем username
                }
                else await AddUser(id_tg, username);                                          // Если нет такой записи - добавляем
            }
        }

        /// <summary>
        /// Возвращает id записи по ИД телеграмма
        /// </summary>
        /// <param name="id_tg"></param>
        /// <returns></returns>
        private async Task<int> CheckID(long id_tg)
        {
            int result = 0;
            try
            {
                _connection.Open();
                string req = "SELECT id FROM newdb.users WHERE id_tg =@id_tg LIMIT 1";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@id_tg", id_tg);
                DbDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows) while (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                _connection.Close();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to get data from the database", exception);
            }
            return result;
        }

        /// <summary>
        /// Обновляет в таблице Users поле никнейм у данной записи
        /// </summary>
        /// <param name="id_record"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task UpdateUsername(long id_record, string? username)
        {
            try
            {
                _connection.Open();
                string req = "UPDATE newdb.users SET username=@username WHERE id=@id_record";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@id_record", id_record);
                command.Parameters.AddWithValue("@username", username);
                await command.ExecuteNonQueryAsync(); //записали
                _connection.Close();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to update data from the database", exception);
            }
        }

        /// <summary>
        /// Получаем из таблицы Users поле Никнейм по номеру записи
        /// </summary>
        /// <param name="num_record"></param>
        /// <returns></returns>
        private async Task<string> GetUsernameFromIdRecord(int num_record)
        {
            string result = "";
            try
            {
                _connection.Open();
                string req = "SELECT username FROM newdb.users WHERE id =@id LIMIT 1";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@id", num_record);
                DbDataReader reader = await command.ExecuteReaderAsync();
                if (reader.HasRows) while (reader.Read())
                    {
                        result = reader.GetString(0);
                    }
                _connection.Close();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to get data from the database", exception);
            }
            return result;
        }

        /// <summary>
        /// Получаем из таблице Users словарь, со всеми ID Telegram`a и Username пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<long, string?>> GetIdTgAndUsername()
        {
            Dictionary<long, string?> result = new Dictionary<long, string?>();
            try
            {
                using (_connection)
                {
                    _connection.Open();
                    string req = "SELECT id_tg, username FROM newdb.users";
                    MySqlCommand command = new MySqlCommand(req, _connection);
                    DbDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows) while (reader.Read())
                        {
                            result.Add(reader.GetInt64(0), reader[1] == DBNull.Value ? null : (string?)reader[1]);
                        }
                    _connection.Close();
                }
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to update data from the database", exception);
            }
            return result;
        }

        /// <summary>
        /// Добавляет в таблицу Users новую запись
        /// </summary>
        /// <param name="id_tg"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task AddUser(long id_tg, string? username)
        {
            try
            {
                _connection.Open();
                string req = "INSERT INTO newdb.users (id_tg, username) VALUES (@id_tg,@username)";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@id_tg", id_tg);
                command.Parameters.AddWithValue("@username", username);
                await command.ExecuteNonQueryAsync(); //записали
                _connection.Close();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to set data from the database", exception);
            }
        }
    }
}
