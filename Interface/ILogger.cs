using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoemaBotTelegram.LoggerService;

namespace PoemaBotTelegram.Interface
{
    public interface ILogger
    {
        public void Log(LogLevel level, string message, Exception exception = null);
        
        void Trace(string message);
        
        void LogInformation (string message);
        
        void LogWarning (string message);
        
        void LogError(string message);
        
        void LogCritical(string message);

    }
}
