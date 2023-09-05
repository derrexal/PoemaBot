using MySql.Data.MySqlClient;
using PoemaBotTelegram.DataProvider;

namespace PoemaBotTelegram.SQL
{
    internal class PoemasCategoryTable:ATable
    {
        protected override string tableName { get; }

        internal PoemasCategoryTable()
        {
            tableName = Settings.POEMAS_CATEGORY_TABLE_NAME;
        }

        /// <summary>
        /// Добавляем новую запись в таблицу "Стихи и категории"
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        private async Task AddRecord(PoemaCategory poemaCategory)
        {
            try
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    await db.PoemasCategorys.AddAsync(poemaCategory);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to ADD RECORD POEMAandCATEGORY from the database");
                throw;
            }
        }   

        internal async Task AddAllRecord(int _poemasID,List<int> _category_numbers)
        {
            foreach(var _categoryID in _category_numbers)
            {
                var poemaCategory = new PoemaCategory { poemaId = _poemasID,categoryId = _categoryID };
                try { await AddRecord(poemaCategory); }
                catch { throw; }
            }
        }
    }
}
