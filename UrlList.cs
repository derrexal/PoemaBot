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

        internal async Task UrlListToSQL ()
        {
            UrlTable urlTable = new UrlTable();
            Url url = new Url();
            try
            {
                using (StreamReader fs = new StreamReader(_path))
                {
                    while (true)
                    {
                        // Читаем строку из файла во временную переменную.
                        string? temp = fs.ReadLine();
                        // Если достигнут конец файла, прерываем считывание.
                        if (temp == null) break;
                        // Пишем считанную строку в итоговую переменную.
                        url.url = temp;
                        // Записываем в базу
                        await urlTable.AddUrl(url);
                    }
                }
                Console.WriteLine("Success record url's");
            }
            catch {throw;}
        }
    }
}
