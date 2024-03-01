namespace PoemaBotTelegram
{
    internal class Settings
    {   
        //NOTE: Долой с глаз от OpenSource
        internal const string TOKEN = "token";

        internal const string ADDRESS = "11.111.11.111";
        internal const uint PORT = 3306;
        
        internal const string DB_USERNAME = "user";
        internal const string DB_PASSWORD = "pass";
        internal const string SSH_USERNAME = "admin";
        internal const string SSH_PASSWORD= "password";

        internal const string CATEGORY_TABLE_NAME = "newdb.category";
        internal const string POEMAS_CATEGORY_TABLE_NAME = "newdb.poemas_category";
        internal const string POEMAS_TABLE_NAME = "newdb.poemas";
        internal const string URL_TABLE_NAME = "newdb.url";
        internal const string USERS_TABLE_NAME = "newdb.users";

        internal const uint MAX_NUM_POEMAS = 100000;
    }
}
