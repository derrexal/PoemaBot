namespace PoemaBotTelegram.DataProvider
{
    public class Poema // модель таблицы в БД
    {
        private int id { get;}
        public string author { get; set; }
        public string title { get; set; }
        public string text { get; set; }
    }
}
