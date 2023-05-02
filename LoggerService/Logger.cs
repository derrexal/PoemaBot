using Microsoft.Extensions.Logging;
using PoemaBotTelegram.Interface;

namespace PoemaBotTelegram.LoggerService
{
    public class Logger 
    {
        private readonly IFileService _fileService;

        public Logger(IFileService fileService)
        {
            _fileService = fileService;
        }

        public void Log(LogLevel level, string message, Exception exception = null)
        {
            var response = CastMessage(message, level, exception);

            switch (level)
            {
                case LogLevel.Trace:
                    Trace(response); // Оставлю на случай, если будет текст разного цвета или т.д.
                    break;
                case LogLevel.Information:
                    LogInformation(response);
                    break;
                case LogLevel.Warning:
                    LogWarning(response);
                    break;
                case LogLevel.Error:
                    LogError(response);
                    break;
                case LogLevel.Critical:
                    LogCritical(response);
                    break;
            }
        }

        private string CastMessage(string message, LogLevel level, Exception exception)
        {
            string currentTime = DateTime.Now.ToString();
            if (exception != null) return $"{currentTime}|{level.ToString()}|{message}{exception}";
            return $"{currentTime}|{level.ToString()}|{message}";
        }

        private void Trace(string message)
        {
            _fileService.WriteLog(message);
        }

        private void LogInformation(string message)
        {
            _fileService.WriteLog(message);
        }

        private void LogWarning(string message)
        {
            _fileService.WriteLog(message);
        }

        private void LogError(string message)
        {
            _fileService.WriteLog(message);
        }

        private void LogCritical(string message)
        {
            _fileService.WriteLog(message);
        }
    }
}
