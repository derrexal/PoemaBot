using Telegram.Bot.Types.ReplyMarkups;

namespace PoemaBotTelegram.TgBotCLient
{
    internal class Constants
    {

        /* Кнопки */
        internal readonly static string buttonGetPoema = "Читать " + char.ConvertFromUtf32(0x1F3B2); // :game_die:    
        internal const string buttonSetting1 = "Первая настройка";
        internal const string buttonSetting2 = "Вторая настройка";
        
        /* Команды */
        internal const string commandStart = "/start";
        internal const string commandAbout = "/about";
        internal const string commandDonate = "/donate";
        internal const string commandSettings = "/settings";
        
        /* Ответы пользователям */
        internal readonly static string messageStart = "Привет, я поэтический бот." + Environment.NewLine +
                    "Готов погрузиться в мир русской поэзии?" + Environment.NewLine +
                    "Нажимая кнопку '" + buttonGetPoema + "' ты подписываешься на ежедневную рассылку стихов.";
        internal readonly static string messageAbout = "По всем вопросам:" + Environment.NewLine + "[Admin](t.me/Jonn_W)";
        internal const string messageDonate = "[QIWI](qiwi.com/n/COLDE034)";
        internal const string messageSorry = "Извини, но пока я умею не так много";
        internal const string messageError = "Ошибка на нашей стороне - уже разбираемся";

        /* Системные */
        internal const int adminId = 1484084809; // сюда присылаем всякие ошибки
        internal const string shedulerTime = "7:00"; // Время в которое бот отправляет стих
        internal const int limitMessageTg = 4096; // Лимит Телеграмма на размер сообщения

        /* Клавиатуры */
        internal readonly static ReplyKeyboardMarkup defaultKeyboardMarkup = new(new[]{new KeyboardButton[] { buttonGetPoema } })
        { // стандартная клавиатура с кнопкой "Читать"
            ResizeKeyboard = true
        };
        internal readonly static ReplyKeyboardMarkup settingsKeyboardMarkup = new(new[] { new KeyboardButton[] { buttonSetting1,buttonSetting2,buttonGetPoema } })
        {// клавиатура для настроек
            ResizeKeyboard = true
        };
    }
}
