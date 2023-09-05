using PoemaBotTelegram.DataProvider;
using NLog;
using PoemaBotTelegram.SQL;

namespace PoemaBotTelegram
{
    internal static class Parser
    {
        private static Logger logger;
        private static DB db;

        static Parser()
        {
            logger = LogManager.GetCurrentClassLogger();
            db = new DB();
        }
        /// <summary>
        /// Записывает стихотворения и его атрибуты в базу
        /// </summary>
        /// <returns></returns>
        internal static async Task RecordAllPoemaToDB()
        {
            logger.Info("Запуск парсера");
            try
            {
                //while (true) { await RecordPoemaToDB(); }
            }
            catch (Exception exception) { logger.Error(exception, "ERROR Record All Poema to DB"); }
        }

        //internal static async Task<bool> RecordPoemaToDB()
        //{
        //    try
        //    {
        //        Url recordUrl = await db.urlTable.GetOneUrlAsync(); // Получаем ссылку для парсинга и устанавливаем статус на "В процессе"
        //        logger.Info("Успешно получили ссылку из базы:" + recordUrl.url.ToString());
                    
        //        var page = new PagePoema(recordUrl.url);
        //        await page.Pars();    // Парсим страницу со стихом
        //        ulong idRecordPoema = await db.poemasTable.AddPoemasAsync(page.Poem);  // Добавляем в базу стих и возвращаем его ID
        //        logger.Info("Успешно спарсили стих в базу: ", page.Poem.Title);
                
<<<<<<< Updated upstream
                var page = new PagePoema(recordUrl.url);
                await page.Pars();    // Парсим страницу со стихом
                ulong idRecordPoema = await db.poemasTable.GetAddPoemasID(page.Poem);  // Добавляем в базу стих и возвращаем его ID
                logger.Info("Успешно спарсили стих в базу: ", page.Poem.Title);
                
                List<ulong> LcategoryID = await db.categoryTable.AddAllCategory(page.LCategory); // Записываем все категории стиха в базу и возвращаем список ID этих категорий
                await db.poemasCategoryTable.AddAllRecord(idRecordPoema, LcategoryID); // Записываем номер стиха и айдишник категории из прошлой таблицы в "Стихи и Категории"
                logger.Info("Успешно добавили категории стиха в таблицу");
                await db.urlTable.UpdateStatusUrlSuccess(recordUrl.Id);              // Обновляем статус этой ссылки на "Выполнено"
                logger.Info("Обновили статус ссылки");
                return true;
            }
            catch{ throw; }
        }
=======
        //        List<ulong> LcategoryID = await db.categoryTable.AddAllCategory(page.LCategory); // Записываем все категории стиха в базу и возвращаем список ID этих категорий
        //        await db.poemasCategoryTable.AddAllRecord(idRecordPoema, LcategoryID); // Записываем номер стиха и айдишник категории из прошлой таблицы в "Стихи и Категории"
        //        logger.Info("Успешно добавили категории стиха в таблицу");
        //        await db.urlTable.UpdateStatusUrlSuccess(recordUrl.Id);              // Обновляем статус этой ссылки на "Выполнено"
        //        logger.Info("Обновили статус ссылки");
        //        return true;
        //    }
        //    catch{ throw; }
        //}
>>>>>>> Stashed changes
    }
}