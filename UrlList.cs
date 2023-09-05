using PoemaBotTelegram.DataProvider;
using PoemaBotTelegram.SQL;

namespace PoemaBotTelegram
{
    internal class UrlList
    {
        private readonly string _path;

        internal UrlList(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Записывает из txt в DB ссылки на стихи
        /// </summary>
        /// <returns></returns>
        internal async Task UrlListToSQL ()
        {
            var db = new DB();
            Url url = new Url();
            try
            {
                using (StreamReader fs = new StreamReader(_path))
                {
                    while (true)
                    {
                        var temp = await fs.ReadLineAsync(); // Читаем строку из файла во временную переменную.
                        if (temp == null) break;  // Если достигнут конец файла, прерываем считывание.
                        url.url = temp; // Пишем считанную строку в итоговую переменную.
                        await db.urlTable.AddUrlAsync(url); // Записываем в базу
                    }
                }
                Console.WriteLine("Success record url's");
            }
            catch {throw;}
        }
    }
}
