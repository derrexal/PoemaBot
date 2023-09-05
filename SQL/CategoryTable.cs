using MySql.Data.MySqlClient;
using PoemaBotTelegram.DataProvider;

namespace PoemaBotTelegram.SQL
{
    internal class CategoryTable:ATable
    {
        protected override string tableName { get; }

        internal CategoryTable( )
        {
            tableName = Settings.CATEGORY_TABLE_NAME;
        }

        /// <summary>
        /// Проверяет категорию на существования в таблице
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private async Task<bool> CheckCategory(Category category)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    if (db.Categorys.Any(c => c.Name == category.Name)) return true;
                    return false;                    
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to CHECK CATEGORY EXISTS from the database");
                throw;
            }
        }

        /// <summary>
        /// Добавляем весь список категорий в базу
        /// </summary>
        /// <param name="allCategory"></param>
        /// <returns></returns>
        internal async Task<IEnumerable<int>> AddAllCategory(IEnumerable<Category> allCategory)
        {
            var numbersRecords = new List<int>();
            try
            {
                foreach (var category in allCategory)
                {
                    if (await CheckCategory(category) == false) // Если такой категории нет в таблице "Категории" - добавляем
                        numbersRecords.Add(await GetAddCategoryID(category));
                }
                return numbersRecords;
            }
            catch { throw; }
        }

        /// <summary>
        /// Добавляем новую запись в таблицу "Категории" и возвращаем её ID
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private async Task<int> GetAddCategoryID(Category category)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    await db.Categorys.AddAsync(category);
                    await db.SaveChangesAsync();
                    var lastCategory = db.Categorys.LastOrDefault();
                    if (lastCategory == null) throw new Exception("No relevant lastPoema in the DB");
                    return lastCategory.Id;
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to ADD CATEGORY from the database");
                throw;
            }
        }
    }
}
