namespace PoemaBotTelegram
{
    internal class Program
    {
        private static void Main()
        {
           Run();
        }

        private static async Task Run()
        {
            await new UrlApp().GetAllUrlFromRedirect();
            var client = new BotClient(Settings.TOKEN);
            
            client.Start();                 // Запускаем бота 
            client.SheduledSendMessage().ConfigureAwait(false).GetAwaiter().GetResult();   // Отправляет стих, если время - 7:00, иначе - ждет.
            //Parser.RecordAllPoemaToDB();    // Парсим стихи с сайта в базу

        }
    }

    //ToDo: добавить к классу Poemas столбец с ID ссылки в таблице Url - Или в таблице url?
}

            