using MySql.Data.MySqlClient;
using System.Data.Common;
using PoemaBotTelegram.DataProvider;


namespace PoemaBotTelegram.SQL
{
    internal class PoemasTable:ATable
    {
        protected override string tableName { get; }
        internal int countPoemas { get { return countPoemas; } private set { value = GetCountPoemas(); } }
        internal PoemasTable()
        {
            tableName = Settings.POEMAS_TABLE_NAME;
        }


        /// <summary>
        /// Добавляет в таблицу Poemas запись
        /// </summary>
        /// <param name="poema"></param>
        /// <returns></returns>
<<<<<<< Updated upstream
        internal async Task<ulong> GetAddPoemasID(Poema poema)
=======
        internal async Task<int> AddPoemasAsync(Poema poema)
>>>>>>> Stashed changes
        {
            try
            {
                using (connection)
                {
<<<<<<< Updated upstream
                    await connection.OpenAsync();
                    string req = $"INSERT INTO {tableName} (author, title, text) VALUES ('{poema.Author}','{poema.Title}','{poema.Text}')";
                    MySqlCommand command = new MySqlCommand(req, connection);
                    await command.ExecuteNonQueryAsync(); //записали
                    await connection.CloseAsync();

                    await connection.OpenAsync();
                    command.CommandText = "SELECT LAST_INSERT_ID()";
                    return (ulong)command.ExecuteScalar();
=======
                    await db.Poemas.AddAsync(poema);
                    await db.SaveChangesAsync();
                    var lastPoema = db.Poemas.LastOrDefault();
                    if (lastPoema == null) throw new Exception("No relevant lastPoema in the DB");
                    return lastPoema.Id;
>>>>>>> Stashed changes
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to set POEMA from the database");
                throw;
            }
        }

        /// <summary>
        /// Получаем из таблицы Poemas одну запись
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Poema> GetPoema(long id)
        {        
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var poema = db.Poemas.FirstOrDefault(u => u.Id == id);
                    if (poema == null) throw new Exception("No relevant poema in the DB");
                    return poema;
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to get POEMA from the database");
                throw;
            }
        }

        /// <summary>
        /// Получает количество записей в таблице
        /// </summary>
        /// <returns></returns>
        private int GetCountPoemas()
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var countPoemas = db.Poemas.Count();
                    return countPoemas;
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to get COUNT POEMA from the database");
                throw;
            }
        }
    }
}
