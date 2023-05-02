using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemaBotTelegram.Interface;

namespace PoemaBotTelegram.LoggerService
{
    public class FileService : IFileService
    {
        private const string PATH = @"C:\Users\Ermol\source\repos\PoemaBotTelegram\PoemaBotTelegram\Files\Log.txt"; // Что означает @? 

        /// <summary>
        ///  Записывает информацию в файл
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public async Task WriteLog(string message)
        {
            using (StreamWriter sw = new StreamWriter(PATH, true, Encoding.Default))
            {
                await sw.WriteLineAsync(message);
            }
        }

        /// <summary>
        /// Читает информацию из файла
        /// </summary>
        /// <returns></returns>
        public async Task ReadLogs()
        {
            using (StreamReader sr = new StreamReader(PATH))
            {
                Console.WriteLine(await sr.ReadToEndAsync());
            }
        }
    }
}