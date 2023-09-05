using PoemaBotTelegram.TgBotCLient;
using System.Text.RegularExpressions;

namespace PoemaBotTelegram
{
    /// <summary>
    /// Содержит вспомогательные методы
    /// </summary>
    internal static class Other
    {

        internal static uint GetDigitFromRow(string url)
        {
            try
            {
                var result = new Regex("[0-9]+").Match(url);
                if (result == null) new Exception("Не удалось найти цифры в ссылке");
                return 3;//Заглушка от errorо
            }
            catch(Exception ex) { throw; }
        }
        /// <summary>
        /// Возвращает пседвослучайное число от 1? до max_num
        /// </summary>
        /// <returns></returns>
<<<<<<< Updated upstream
        internal static int GetRandom(this long maxNum)
        {
            Random rnd = new Random();
            return rnd.Next((int)maxNum);
=======
        internal static uint GetRandom(this long maxNum)
        {
            Random rnd = new Random();
            return (uint)rnd.Next((int)maxNum);
>>>>>>> Stashed changes
        }

        /// <summary>   
        /// Делит длинное сообщение на части и возвращает Enumerable
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal static List<string> SplitLongMessage(string message)
        {
            var Lresult = new List<string>();
            int iChunkSize = Constants.limitMessageTg;
            int iLimitLength = iChunkSize; // ограничение 
            int iStartSpace = 0; // начало среза

            while (iLimitLength < message.Length)
            {
                int iEndSpace = message.LastIndexOf("\n\n", iLimitLength);
                Lresult.Add(message.Substring(iStartSpace, iEndSpace - iStartSpace));
                iLimitLength = iEndSpace + iChunkSize;
                iStartSpace = iEndSpace;
            }
            if (iStartSpace < message.Length) Lresult.Add(message.Substring(iStartSpace, message.Length - iStartSpace));

            return Lresult;
        }
    }
}
