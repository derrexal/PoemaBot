using PoemaBotTelegram.TgBotCLient;

namespace PoemaBotTelegram
{
    class Program
    {
        static async Task Main(string[] args)
        {
            BotClient client = new BotClient(Settings.token);
            Owner owner = new Owner();

            client.StartBot(); // Запускаем бота 
            //await owner.RecordPoemaToSQL(); // Парсим стихи с сайта в базу
            //client.SheduledSendMessage(); // Отправляет стих, если время - 7:00, иначе - ждет.
            
            Console.ReadLine();
        }
    }
}
