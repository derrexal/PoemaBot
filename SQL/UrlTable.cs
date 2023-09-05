using PoemaBotTelegram.DataProvider;

namespace PoemaBotTelegram.SQL
{
    internal class UrlTable:ATable
    {        
        protected override string tableName { get; }

        internal UrlTable()
        {
            tableName = Settings.URL_TABLE_NAME;
        }

        /// <summary>
        /// Добавляет в таблицу URL 1 запись
        /// </summary>
        /// <param name="poema"></param>
        /// <returns></returns>
        internal async Task AddUrlAsync(Url url)
        {
            try 
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    await db.Urls.AddAsync(url);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to add URL from the database");
                throw;
            }
        }

        /// <summary>
        /// Получаем из таблицы URL одну запись
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal async Task<Url> GetOneUrlAsync()
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
<<<<<<< Updated upstream
                    await connection.OpenAsync();
                    string req = $"SELECT id,url FROM {tableName} WHERE status ='{Constants.statusNone}' LIMIT 1";
                    MySqlCommand command = new MySqlCommand(req, connection);
                        DbDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows) while (reader.Read())
                        {
                            result.Id = reader.GetInt32(0);
                            result.url = reader.GetString(1);
                        }
                    else throw new Exception("No relevant URL in the DB");
                    
                    await connection.CloseAsync();
                    await connection.OpenAsync();
                    command.CommandText = $"UPDATE {tableName} SET status='{Constants.statusInProcess}' WHERE id={result.Id}";
                    command.ExecuteNonQuery();
                    result.Status = Constants.statusInProcess;
                    return result;
=======
                    var url = db.Urls.FirstOrDefault(u => u.Status == Constants.statusNone);
                    if (url == null) throw new Exception("No relevant URL in the DB");
                    url.Status = Constants.statusInProcess;
                    await db.SaveChangesAsync();
                    return url;
>>>>>>> Stashed changes
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to get URL from the database");
                throw;
            }
        }

        /// <summary>
        /// Устанавливает в поле status таблицы URL значение - успешно 
        /// </summary>
        /// <param name="poema"></param>
        /// <returns></returns>
<<<<<<< Updated upstream
        internal async Task UpdateStatusUrlSuccess(int id)
=======
        internal async Task UpdateStatusUrlSuccessAsync(int id)
>>>>>>> Stashed changes
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var url = db.Urls.FirstOrDefault(u => u.Id == id);
                    if (url == null) throw new Exception("No relevant URL in the DB");
                    url.Status = Constants.statusSuccess;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to set status URL from the database");
                throw;
            }
        }

        public bool CheckUrl(Url url)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var urlDb = (db.Urls.FirstOrDefault(x => x.url == url.url));
                    if (urlDb == null) return false;
                    return true;
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to CHECK URL from the database");
                throw;
            }
        }

        public uint GetMaxNumUrl()
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var urlDb = db.Urls.LastOrDefault();
                    
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to Get Max Num Url from the database");
                throw;
            }
        }
    }
}
}