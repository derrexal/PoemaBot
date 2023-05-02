using AngleSharp;
using AngleSharp.Dom;
using PoemaBotTelegram.DataProvider;
using PoemaBotTelegram.LoggerService;

namespace PoemaBotTelegram
{
    internal class PagePoema
    {
        private readonly string _baseUrl = "https://www.culture.ru/poems"; // начало ссылки для парсинга стихов (в файле url_list.txt хранятся строки с продолжением этой ссылки)
        private readonly string _sellectorAuthor = "div.IHYwd"; // Селлекторы для поиска ключевых элементов на странице
        private readonly string _sellectorTitle = "div.xtEsw";
        private readonly string _sellectorText =  "div.xZmPc p";
        private readonly string _sellectorCategory = "div.WrMwB a.HEX1L";

        private readonly string _urlPoema; // ссылка по которой находится конкретный стих
        private readonly Logger _logger;
        private Poema _poema;
        private List<Category> _lCategory;

        public List<Category> LCategory
        {
            get { return _lCategory; }
            private set { _lCategory = value; }
        }
        internal Poema Poem
        {
            get { return _poema; }
            private set { _poema = value; }
        }

        internal PagePoema(string url)
        {
            _urlPoema = _baseUrl + url;
            _logger = new Logger(new FileService());
            Poem = new Poema();
            LCategory = new List<Category>();
        }

        /// <summary>
        ///  Парсит страничку и устанавливает атрибуты стиха и список категорий
        /// </summary>
        /// <returns></returns>
        internal async Task Pars()
        {
            try
            {
                await SetPoema();
                await SetCategory();
            }
            catch { throw; }
        }
        
        /// <summary>
        /// Возвращает элемент с заданным селектором со страницы
        /// </summary>
        /// <param name="cellSelector"></param>
        /// <returns></returns>
        private async Task<List<string>> GetElement(string cellSelector)
        {
            List<string> result = new List<string>();
            try
            {
                IConfiguration config = Configuration.Default.WithDefaultLoader();
                IBrowsingContext context = BrowsingContext.New(config);
                IDocument document = await context.OpenAsync(_urlPoema);
                IHtmlCollection<IElement> cells = document.QuerySelectorAll(cellSelector);
                IEnumerable<string> titles = cells.Select(m => m.InnerHtml);
                for (int i = 0; i < titles.Count(); i++)
                {
                    result.Add(titles.ElementAt(i));  // когда парсим текст - много раз забегает в строчку 29 на этой проверке. Альтернативы?
                    //if ((titles.Count() > 1) & (i < titles.Count() - 1)) result.Add("\n" + "\n");
                }
            }
            catch (Exception exception)
            {
                _logger.Log(LogLevel.Error, "Unable to get data from the site", exception);
                throw;
            }
            return result;
        }

        /// <summary>
        /// Устанавливаем поля объекта _poema
        /// </summary>
        private async Task SetPoema()
        {
            try
            { // Далее в каждом случае - соединяем список в строку
                Poem.author =String.Join("", await GetElement(_sellectorAuthor));
                Poem.title = String.Join("", await GetElement(_sellectorTitle));
                Poem.text = String.Join("\n\n", await GetElement(_sellectorText));    
            }
            catch { throw; }
        }

        /// <summary>
        /// Заполняет список категориями стихотворения
        /// </summary>
        /// <returns></returns>
        private async Task SetCategory()
        {
            try
            {
                Category category = new Category();
                List<Category> result = new List<Category>();
                List<string> list = await GetElement(_sellectorCategory);      
                foreach (string item in list)
                {
                    category.name = item;
                    result.Add(category);
                }
            }
            catch { throw; }
        }

    }
}
