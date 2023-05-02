using MySqlX.XDevAPI.Common;
using PoemaBotTelegram.DataProvider;
using PoemaBotTelegram.LoggerService;
using PoemaBotTelegram.SQL;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using TGConstants = PoemaBotTelegram.TgBotCLient.Constants;


/// Добавить перегрузки в метод отправки сообщений? 
/// !!!!!
namespace PoemaBotTelegram
{
    internal class BotClient
    {
        private readonly TelegramBotClient _client;         // { Timeout = TimeSpan.FromSeconds(10)};
        private readonly Logger _logger;

        internal BotClient(string token)
        {
            _client = new TelegramBotClient(token);
            _logger = new Logger(new FileService());
        }

        /// <summary>
        /// Запускает работу бота
        /// </summary>
        internal void StartBot()
        {
            _client.StartReceiving(Update, Error);
        }

        /// <summary>
        /// Обрабатывает сообщение пользователя
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            if ((update.Message == null) || (update.Message.Text == null)) return; // Если сообщение пусто - выходим (а доходит ли он когда-нибудь до второго условия?)
            Message message = update.Message;
            string textMessage = message.Text;
            long id = message.Chat.Id;
            string? username = message.Chat.Username;
            UsersTable usersTable = new UsersTable();

            try
            {
                //записываем пользователя в бд, если такого ещё нет (ID пользоваетеля, никнейм)
                await usersTable.CheckAndAddUser(id, username);
                // Бот "Печатает"
                await botClient.SendChatActionAsync(id, Telegram.Bot.Types.Enums.ChatAction.Typing);

                // Обратаывает ввод пользователя
                if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    if (message.Text == TGConstants.buttonGetPoema)
                    {
                        await SendMessage(botClient, await GetPoemaFromDB(), id, username, textMessage);
                        return;
                    }
                    else if (message.Text.ToLower() == TGConstants.commandStart)  // обрабатываем сообщения от пользователей
                    {
                        await SendMessage(botClient, TGConstants.messageStart, id, username, textMessage);
                        return;
                    }
                    else if (message.Text.ToLower() == TGConstants.commandDonate)
                    {
                        await SendMessage(botClient, TGConstants.messageDonate, id, username, textMessage);
                        return;
                    }
                    else if (message.Text.ToLower() == TGConstants.commandAbout)
                    {
                        await SendMessage(botClient, TGConstants.messageAbout, id, username, textMessage);
                        return;
                    }
                    else if (message.Text.ToLower() == TGConstants.commandSettings)
                    {
                        await SendKeybordMarkup(botClient, TGConstants.settingsKeyboardMarkup, id, username, textMessage);
                        return;
                    }
                }
                await SendMessage(botClient, TGConstants.messageSorry, id, username, textMessage);
                return;
            }
            catch (Exception exception)
            {
                string text = id.ToString() + username + textMessage;
                _logger.Log(LogLevel.Error, text, exception);
                await SendMessage(botClient, TGConstants.messageError, id, username, textMessage);
            }
            return;
        }

        /// <summary>
        /// Обрабатываем ошибки бота
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task Error(ITelegramBotClient botClient, Exception exception, CancellationToken token)
        {
            /// После того как зашел в эту процедуру - не заходит в остальные апдейты - или заходит?
            // В каком случае сюда вообще заходит?
            _logger.Log(LogLevel.Critical, "Критическая ошибка(Попали в блок ERROR TGBOTa)", exception);
            await SendMessage(botClient, TGConstants.messageError, TGConstants.adminID);
        }

        /// <summary>
        /// Отправляет текстовый ответ пользователю. Если оно больше ограничения тг - разбивает на части
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="replyKeyboardMarkup"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task SendMessage(ITelegramBotClient botClient, string message, long id, string? username = "", string userMessage = "")
        {
            string textLog = id.ToString() + "|" + username + "|" + userMessage + "|" + message.Split('\n')[0] + "| ";
            try
            {
                if (message.Length == 0) throw new Exception("Ошибка получения стиха, вероятно его нет в базе");
                if (message.Length > TGConstants.LimitMessageTg)
                {
                    IEnumerable<string> words = Owner.Split(message, TGConstants.LimitMessageTg);
                    foreach (string word in words) await SendMessage(botClient, word, id, username, userMessage);
                    return;
                }
                await botClient.SendTextMessageAsync(id, message, Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: TGConstants.defaultKeyboardMarkup); //формат показа пользователю
                _logger.Log(LogLevel.Trace, textLog);
            }

            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, textLog, exception);
                throw;
            }
            return;
        }

        /// <summary>
        /// Отправляет пользователю клавиатуру "Настройки"
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="keyboardMarkup"></param>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="userMessage"></param>
        /// <returns></returns>
        private async Task SendKeybordMarkup(ITelegramBotClient botClient, ReplyKeyboardMarkup keyboardMarkup, long id, string? username = "", string userMessage = "")
        {
            string textLog = id.ToString() + "|" + username + "|" + userMessage + "| ";

            try
            {
                await botClient.SendTextMessageAsync(id, "123", replyMarkup: keyboardMarkup);
            }

            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, textLog, exception);
                throw;
            }
            return;
        }

        /// <summary>
        /// Постоянно проверяет время и если оно оказалось равно 07:00, - отправляет сообщение и замолкает почти на сутки
        /// </summary>
        internal async Task SheduledSendMessage()
        {

            while (true)
            {
                if (DateTime.Now.ToString("H:mm") == TGConstants.shedulerTime) // Если текущее время равно установленному
                {
                    try
                    {
                        await SendMessageAllUsers();
                        Thread.Sleep(86000000); // почти 1 сутки
                    }

                    catch (Exception exception)
                    {
                        _logger.Log(LogLevel.Error, "Unable to SheduledSendMessage", exception);
                        throw;

                    }
                }
            }
        }

        /// <summary>
        /// Отправляет 1 стихотворение всем пользователям
        /// </summary>
        /// <returns></returns>
        private async Task SendMessageAllUsers()
        {
            UsersTable usersTable = new UsersTable();
            try
            {
                Dictionary<Int64, string?> List_Users = await usersTable.GetIdTgAndUsername();
                string sPoema = await GetPoemaFromDB();

                foreach (var user in List_Users)
                {
                    await SendMessage(_client, sPoema, user.Key, user.Value); // ИД,Никнейм
                }
            }
            catch { throw; }
        }

        private async Task<string> GetPoemaFromDB()
        {
            try
            {
                PoemasTable poemasTable = new PoemasTable();
                int num = Owner.GetRandom(poemasTable._countPoemas);
                Poema poema = await poemasTable.GetRandomPoema(num);
                return GetPoemaToStringMd(poema);
            }
            catch { throw; }
        }

        /// <summary>
        /// Возвращает стих в формате строки для тг бота (HTML)
        /// </summary>
        /// <returns></returns>
        private string GetPoemaToStringMd(Poema poema)
        {
            if (poema.author == null) return "";
            string result = "<b>" + poema.author + " - " + poema.title + "</b>" + "\n" + "\n" + poema.text;
            result = result.Replace("<br>", "\n"); // Эти теги телеграм не поддерживает, приходится обрабатывать ручками
            result = result.Replace("&nbsp;", " ");
            return result;
        }




    }
}