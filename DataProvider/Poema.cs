namespace PoemaBotTelegram.DataProvider
{
    public class Poema // модель таблицы в БД
    {
        private int Id { get;}
        public string Author { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
