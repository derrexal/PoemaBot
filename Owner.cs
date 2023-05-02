using PoemaBotTelegram.DataProvider;
using PoemaBotTelegram.Interface;
using PoemaBotTelegram.LoggerService;
using PoemaBotTelegram.SQL;

namespace PoemaBotTelegram
{
    /// <summary>
    /// Содержит вспомогательные методы
    /// </summary>
    internal class Owner
    {
        private readonly Logger _logger = new Logger(new FileService());

        /// <summary>
        /// Разбивает строку на части, кратные входному параметру
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static List<string> Split(string str, int chunkSize)
        {
            var res = new List<string>();
            for (int i = 0; i < str.Length; i += chunkSize)
            {
                if ((i + chunkSize) <= str.Length) res.Add(str.Substring(i, chunkSize));
                else res.Add(str.Substring(i, str.Length - i));
            }
            return res;
        }

        /// <summary>
        /// Возвращает пседвослучайное число от 1? до max_num
        /// </summary>
        /// <returns></returns>
        public static int GetRandom(int max_num)
        {
            Random rnd = new Random();
            return rnd.Next(max_num);
        }

        /// <summary>
        /// Парсит сайт и записывает стихотворения в базу
        /// </summary>
        /// <returns></returns>
        internal async Task RecordPoemaToSQL()
        {
            try
            {
                UrlTable urlTable = new UrlTable();
                PoemasTable poemasTable = new PoemasTable();
                CategoryTable categoryTable = new CategoryTable();
                PoemasCategoryTable poemasCategoryTable = new PoemasCategoryTable();

                int numNextURL = await urlTable.GetNumberUrl(); 

                for (int i = numNextURL; i <= urlTable._countUrl; i++)
                {
                    Url url = await urlTable.GetOneUrl(i);      // Получаем ссылку для парсинга
                    PagePoema page = new PagePoema(url.url);    // Инициализируем новый объект, который будет парсить страницу со стихом
                    await page.Pars();
                    
                    await poemasTable.AddPoemas(page.Poem);             // Добавляем стих в базу
                    List<int>  numbersCategory = await categoryTable.AddAllCategory(page.LCategory);      // Записываем все категории из списка в таблицу, если таковых нет в ней уже и возвращаем список номеров этих категорий
                    foreach(int idCategory in numbersCategory) await poemasCategoryTable.AddRecord(i,idCategory);       // Записываем номер стиха и айдишник категории из прошлой таблицы в "Стихи и Категории"

                    await urlTable.UpdateStatusUrl(i);              // Обновляем статус этой ссылки на "Выполнено"
                }
            }
            catch(Exception exception)
            {
                _logger.Log(LogLevel.Error, "ERROR Record Poema to SQL", exception);
                await RecordPoemaToSQL();
            }
        }
    }
}
