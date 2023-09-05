using PoemaBotTelegram.DataProvider;
using PoemaBotTelegram.SQL;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TGConstants = PoemaBotTelegram.TgBotCLient.Constants;
using Telegram.Bot.Types.Enums;
using NLog;

namespace PoemaBotTelegram
{
    internal  class BotClient
    {
        private readonly TelegramBotClient client;
        private Logger logger { get; set; }

        internal BotClient(string token)
        {
            client = new TelegramBotClient(token);
            logger = LogManager.GetCurrentClassLogger(); 
        }

        /// <summary>
        /// Запускает работу бота
        /// </summary>
        internal void Start()
        {
            client.StartReceiving(Update, Error);
            logger.Info("Запуск бота");
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
            if (update.Message == null) return; // Если сообщение пусто - выходим

            Message message = update.Message;
            long id = message.Chat.Id;
            string? username = message.Chat.Username; 
            string textMessage = message.Text != null ? message.Text : message.Type.ToString(); // Если вместо текста прислал, н-р фото - запишем Photo
            var db = new DB();
            var poema = GetPoemaFromDB();
            try
            {
                //записываем пользователя в бд, если такого ещё нет (ID пользоваетеля, никнейм)
                await db.usersTable.CheckAndAddUser(id, username);

                // Бот "Печатает"
                await botClient.SendChatActionAsync(id, ChatAction.Typing);

                //NOTE: Удалить ретёрн плохая идея, приведет к вынужденному дублированию кода
                // Ввод пользователя - текст 
                if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    if (textMessage == TGConstants.buttonGetPoema) { // Кнопка "Читать"
                        await SendMessage(botClient, await poema, id, username, textMessage);
                        return; }
                    else if (textMessage.ToLower() == TGConstants.commandStart) { // Старт бота
                        await SendMessage(botClient, TGConstants.messageStart, id, username, textMessage);
                        return; }
                    else if (textMessage.ToLower() == TGConstants.commandDonate) { // Донат
                        await SendMessage(botClient, TGConstants.messageDonate, id, username, textMessage);
                        return; }
                    else if (textMessage.ToLower() == TGConstants.commandAbout) { // Блок информация
                        await SendMessage(botClient, TGConstants.messageAbout, id, username, textMessage);
                        return; }
                    else if (textMessage.ToLower() == TGConstants.commandSettings) {  // Пока не работает
                        await SendKeybordMarkup(botClient, TGConstants.settingsKeyboardMarkup, id, username, textMessage);
                        return; } 
                }
                await SendMessage(botClient, TGConstants.messageSorry, id, username, textMessage); // Пользователь ввел что-то непонятное боту
            }
            ///NOTE: Перенести в блок Error?
            catch (Exception exception)
            {
                logger.Error(exception, $"Ошибка в обработке сообщения пользователя id:{id} nickname:{username} user_message:{textMessage} "); // записываем в лог
                await SendMessage(botClient, TGConstants.messageError, id, username, textMessage); // отправляем пользователю отбивку об ошибке
                await SendMessage(botClient, exception.Source + "\n" + exception.Message.ToString(), TGConstants.adminId); // отправляем админу информацию об ошибке у пользователя
            }
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
            /// Для отслеживания ошибок бота, других от обработки сообщений
            logger.Fatal(exception, "Критическая ошибка в работе бота");
        }   

        /// <summary>
        /// Отправляет текстовый ответ пользователю. Если оно больше ограничения тг - разбивает на части
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="replyKeyboardMarkup"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task SendMessage(ITelegramBotClient botClient, string message, long id, string? username = "", string? userMessage = "")
        {
            try
            {
                if (message.Length > TGConstants.limitMessageTg) // Если сообщение слишком длинное - разделяем его 
                {
                    IEnumerable<string> words = Other.SplitLongMessage(message);
                    foreach (string word in words) await SendMessage(botClient, word, id, username, userMessage);
                    return;
                }
                await botClient.SendTextMessageAsync(id, message,parseMode:ParseMode.Html, replyMarkup: TGConstants.defaultKeyboardMarkup); // формат показа пользователю (HTML)
                logger.Info($"Успешно отправили сообщение пользователю id:{ id } nickname:{ username} user_message:{ userMessage} bot_message: {message.Split('\n')[0]}");
            }
            catch { throw; }
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
        private async Task SendKeybordMarkup(ITelegramBotClient botClient, ReplyKeyboardMarkup keyboardMarkup, long id, string? username = "", string? userMessage = "")
        {
            try
            {
                await botClient.SendTextMessageAsync(id, "1", replyMarkup: keyboardMarkup);
                logger.Info($"Успешно отправили клавиатуру пользователю id:{id} nickname:{username} user_message:{userMessage}");
            }

            catch (Exception exception)
            {
                logger.Error(exception, $"Ошибка отправки клавиатуры пользователю id:{ id } nickname:{ username} user_message:{ userMessage}");
                throw;
            }
            return;
        }

        /// <summary>
        /// Постоянно проверяет время и если оно оказалось равно 07:00, - отправляет сообщение и замолкает почти на сутки
        /// </summary>
        internal async Task SheduledSendMessage()
        {
            var currentTIme = TimeOnly.FromDateTime(DateTime.Now); // время сейчас
            var shedulerTime = new TimeOnly(7, 0, 0);    //07:00 
            var oneDays = 86400000; // 1 сутки в мс
            var differenceTime = (int)(currentTIme - shedulerTime).TotalMilliseconds; // Разница между сейчас и 7 утра в мс

            logger.Info($"Шедулер запустился в {currentTIme}");
            while (true)
            {
                if (currentTIme == shedulerTime)
                {
                    try
                    {
                        await SendMessageAllUsers();
                        logger.Info("Успешно отправили ежедневное стихотворение всем пользователям");
                        await Task.Delay(oneDays); // Остановка метода на onedays
                    }
                    catch (Exception exception)
                    {
                        logger.Error(exception, "Unable to Sheduled Send Message");
                        throw;
                    }
                }
                else await Task.Delay(differenceTime); // Остановка метода на differenceTime 
            }
        }
        
        /// <summary>
        /// Отправляет 1 стихотворение всем пользователям
        /// </summary>
        /// <returns></returns>
        private async Task SendMessageAllUsers()
        {
            try
            {
                var db = new DB();
                string sPoema = await GetPoemaFromDB();
                var List_Users = db.usersTable.GetIdTgAndUsername();
                foreach (var user in List_Users)
                {
                    try { await SendMessage(client, sPoema, user.IdTg, user.Username); } // айди, никнейм
                    catch (Telegram.Bot.Exceptions.ApiRequestException exception)
                    {
                        logger.Error(exception,$"Ошибка отправки стиха по расписанию для всех пользователей. Бот заблокирован пользователем id:{user.IdTg} nickname:{user.Username}");
                    }
                }
            }
            catch { throw; }
            
        }

        /// <summary>
        /// Вовращает стих из базы в виде строки для ТГ
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetPoemaFromDB()
        {
            try
            {
                var db = new DB();
                long num = Other.GetRandom(db.poemasTable.countPoemas);

                Poema poema = await db.poemasTable.GetPoema(num);
                if (poema.Author == null) throw new Exception("Empty Get poema"); // Если стих пустой
                return GetPoemaToStringMd(poema);
            }
            catch { throw; }
        }

        /// <summary>
        /// Преобразует стих в интересующий нас формат (HTML)
        /// </summary>
        /// <returns></returns>
        private string GetPoemaToStringMd(Poema poema)
        {
            string result = "<b>" + poema.Author + " - " + poema.Title + "</b>" + "\n" + "\n" + poema.Text;
            result = result.Replace("<br>", "\n"); // Эти теги телеграм не поддерживает, приходится обрабатывать ручками
            result = result.Replace("&nbsp;", " ");
            return result;
        }
    }
}