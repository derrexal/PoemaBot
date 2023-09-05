using MySql.Data.MySqlClient;
using NLog;

namespace PoemaBotTelegram.SQL
{
    abstract class ATable
    {
<<<<<<< Updated upstream
        protected readonly MySqlConnection connection;

=======
>>>>>>> Stashed changes
        protected abstract string tableName { get; }
        protected Logger logger { get; private set; }

        internal ATable()
        {
            logger = LogManager.GetCurrentClassLogger();
        }


    }
}
