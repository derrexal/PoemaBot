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
    internal class CategoryTable
    {
        private readonly MySqlConnection _connection;
        private readonly Logger _logger;
        private static int _countCategory; // Общее количество категорий в таблице

        public static int CountCategory
        {
            get { return _countCategory; }
        }

        internal CategoryTable()
        {
            _connection = new MySqlConnection(Constants.connectionString);
            _logger = new Logger(new FileService());
            _countCategory = GetCountCategory();
        }

        /// <summary>
        /// Получаем количество записей в таблице "Категории"
        /// </summary>
        /// <returns></returns>
        private int GetCountCategory()
        {
            int result = 0;
            try
            {
                _connection.Open();
                string req = "SELECT COUNT(*) FROM newdb.category";
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
        /// Добавляем новую запись в таблицу "Категории"
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private async Task AddCategory(string category)
        {
            try
            {
                _connection.Open();
                string req = "INSERT INTO newdb.category (name) VALUES (@name);";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@name", category);
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
        /// Проверяет категорию на существования в таблице
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private async Task<bool> CheckCategory(string category)
        {
            int result = 0;
            try
            {
                _connection.Open();
                string req = "SELECT EXISTS(SELECT 1 FROM newdb.category WHERE name = @category LIMIT 1)"; // оператор exists возвращает 1 если такая запись в таблице есть, и возвращает 0 в обратном случае
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@category", category);
                DbDataReader reader = command.ExecuteReader();
                if (reader.HasRows) while (reader.Read()) result = reader.GetInt32(0);
                _connection.Close();
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to get data from the database", exception);
                throw;
            }
            if (result == 1) return true;
            return false;
        }

        /// <summary>
        /// Получаем номер записи в таблице "Категории"
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetNumberRecord(string category)
        {
            int result = 0;
            try
            {
                _connection.Open();
                string req = "SELECT id FROM newdb.category Where name = @name";
                MySqlCommand command = new MySqlCommand(req, _connection);
                command.Parameters.AddWithValue("@name", category);
                DbDataReader reader = command.ExecuteReader();
                if (reader.HasRows) while (reader.Read()) result = reader.GetInt32(0);
                _connection.Close();
                return result;
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to get NUMBER_CATEGORY from the database", exception);
                throw;
            }

        }

        /// <summary>
        /// Добавляем весь список категорий в базу
        /// </summary>
        /// <param name="allCategory"></param>
        /// <returns></returns>
        internal async Task<List<int>> AddAllCategory(List<Category> allCategory)
        {
            List<int> numbersRecords = new List<int>();
            try
            {
                foreach (Category category in allCategory)
                {
                    // Если такой категории нет в таблице "Категории" - добавляем
                    if (await CheckCategory(category.name) == false) await AddCategory(category.name);
                    numbersRecords.Add(await GetNumberRecord(category.name));
                }
            }   
            catch { throw; }
            return numbersRecords;
        }
    }
}
