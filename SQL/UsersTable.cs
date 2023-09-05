using MySql.Data.MySqlClient;
using PoemaBotTelegram.DataProvider;
using System.Data.Common;
using Telegram.Bot.Types;
using User = PoemaBotTelegram.DataProvider.User;

namespace PoemaBotTelegram.SQL
{
    internal class UsersTable:ATable
    {
        protected override string tableName { get; }

        internal UsersTable()
        {
            tableName = Settings.USERS_TABLE_NAME;
        }

        /// <summary>
        /// Если такого юзьверя не добавляли - добавляем. Если есть такой ИД в таблице, но отличается username - обновляем никнейм
        /// </summary>
        /// <param name="id_tg"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task CheckAndAddUser(long idTg, string? username)
        {
            try
            {
                //using (ApplicationContext db = new ApplicationContext())
                //{
                    if(await CheckId(idTg) is int idRecord) // номер записи в таблице
                    if (idRecord != 0)                     // Если в таблице есть такой юзьверь
                    {
                        string usernameRecord = await GetUsernameFromIdRecord(idRecord);
                        if (usernameRecord == username) return;   // Если нашли запись с таким юзьверем - выходим
                        await UpdateUsername(idRecord, username); // Если нашли запись с таким ID но другим username - обновялем username
                    }
                    else await AddUser(idTg, username);           // Если нет такой записи - добавляем
                //}
            }
            catch { throw; }
        }

        /// <summary>
        /// Возвращает id юзера по ИД телеграмма
        /// </summary>
        /// <param name="id_tg"></param>
        /// <returns></returns>
        private async Task<int> CheckId(long idTg)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var user = db.Users.FirstOrDefault(x => x.IdTg == idTg);
                    if (user == null) throw new Exception("No relevant poema in the DB");
                    return user.Id;
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to GET ID USER RECORD from the database");
                throw;
            }
        }

        /// <summary>
        /// Обновляет в таблице Users поле никнейм у данной записи
        /// </summary>
        /// <param name="id_record"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task UpdateUsername(int idRecord, string? username)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var user = db.Users.FirstOrDefault(x => x.Id == idRecord);
                    if (user == null) throw new Exception("No relevant user in the DB");
                    user.Username = username;
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to UPDATE USERNAME from the database");
                throw;
            }
        }

        /// <summary>
        /// Получаем из таблицы Users поле Никнейм по номеру записи
        /// </summary>
        /// <param name="num_record"></param>
        /// <returns></returns>
        private async Task<string> GetUsernameFromIdRecord(int idRecord)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var user = db.Users.FirstOrDefault(x => x.Id == idRecord);
                    if (user == null) throw new Exception("No relevant poema in the DB");
                    return user.Username;

                }
            }
            catch (Exception exception)
            {
                logger.Error("Unable to GET USERNAME FROM ID data from the database", exception);
                throw;
            }
        }
                
        /// <summary>
        /// Получаем из таблице Users словарь, со всеми ID Telegram`a и Username пользователей
        /// </summary>
        /// <returns></returns>
        public List<User> GetIdTgAndUsername()
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var list = db.Users.ToList();
                    return list;                    
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to GET USERs from the database");
                throw;
            }
        }

        /// <summary>
        /// Добавляет в таблицу Users новую запись
        /// </summary>
        /// <param name="id_tg"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task AddUser(long idTg, string? username)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    var user = new User { IdTg = idTg, Username = username };
                    await db.Users.AddAsync(user);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to ADD USER from the database");
                throw;
            }
        }
    }
}