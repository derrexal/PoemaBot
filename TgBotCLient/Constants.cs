using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.WebRequestMethods;

namespace PoemaBotTelegram.TgBotCLient
{
    internal class Constants
    {
        internal readonly static string buttonGetPoema = "Читать " + char.ConvertFromUtf32(0x1F3B2); // :game_die:    
        internal readonly static string buttonSetting1 = "Первая настройка";
        internal readonly static string buttonSetting2 = "Вторая настройка";
        
        internal readonly static string commandStart = "/start";
        internal readonly static string commandAbout = "/about";
        internal readonly static string commandDonate = "/donate";
        internal readonly static string commandSettings = "/settings";
        
        internal readonly static string messageStart = "Привет, я поэтический бот." + Environment.NewLine +
                    "Готов погрузиться в мир русской поэзии?" + Environment.NewLine +
                    "Нажимая кнопку '" + buttonGetPoema + "' ты подписываешься на ежедневную рассылку стихов.";
        internal readonly static string messageAbout = "По всем вопросам:" + Environment.NewLine + "[Admin](t.me/Jonn_W)";
        internal readonly static string messageDonate = "[QIWI](qiwi.com/n/COLDE034)";
        internal readonly static string messageSorry = "Извини, но пока я умею не так много";
        internal readonly static string messageError = "Ошибка на нашей стороне - уже разбираемся";

        internal readonly static int adminID = 1484084809;
        internal readonly static string shedulerTime = "07:00";
        internal readonly static int LimitMessageTg = 4096; // Лимит Телеграмма на размер сообщения

        internal readonly static ReplyKeyboardMarkup defaultKeyboardMarkup = new(new[]{new KeyboardButton[] { buttonGetPoema } })
        {
            ResizeKeyboard = true
        };

        internal readonly static ReplyKeyboardMarkup settingsKeyboardMarkup = new(new[] { new KeyboardButton[] { buttonSetting1,buttonSetting2,buttonGetPoema } })
        {
            ResizeKeyboard = true
        };
    }
}
