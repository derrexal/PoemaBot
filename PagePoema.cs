using AngleSharp;
using AngleSharp.Dom;
using NLog;
using PoemaBotTelegram.DataProvider;
using System.Net;
using System.Net.Http.Headers;
using System.Web;

namespace PoemaBotTelegram
{
    internal class PagePoema
    {
        private const string _sellectorAuthor = "div.IHYwd"; // Селлекторы для поиска ключевых элементов на странице
        private const string _sellectorTitle = "div.xtEsw";
        private const string _sellectorText =  "div.xZmPc p";
        private const string _sellectorCategory = "div.WrMwB a.HEX1L";
        
        private static Logger logger;
        private readonly string _fullUrl; // Cсылка по которой находится конкретный стих
        private IDocument pageDocument;
        private Poema _poema;
        private List<string> _lCategory;

        internal IDocument PageDocument
        {
            get { return pageDocument; }
            private set { pageDocument = value; }
        }

        internal Poema Poem
        {
            get { return _poema; }
            private set { _poema = value; }
        }

        internal List<string> LCategory
        { 
            get { return _lCategory; }
            private set { _lCategory = value; }
        }

        internal PagePoema(string url)
        {
            _fullUrl = url;
            Poem = new Poema();
            LCategory = new List<string>();
            pageDocument = GetDocumentAsync();
            logger = LogManager.GetCurrentClassLogger();
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
        /// Возвращает страничку которую будем распарсивать
        /// </summary>
        /// <returns></returns>
        private IDocument GetDocumentAsync()
        {
            try 
            {
                IConfiguration config = Configuration.Default.WithDefaultLoader();
                IBrowsingContext context = BrowsingContext.New(config);
                IDocument doc = context.OpenAsync(_fullUrl).Result; // Result - Это плохо, т.к. приводит к блокировке 
                return doc;
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
                IHtmlCollection<IElement> cells = pageDocument.QuerySelectorAll(cellSelector);
                IEnumerable<string> titles = cells.Select(m => m.InnerHtml);
                for (int i = 0; i < titles.Count(); i++)
                {
                    // NOTE: Kогда парсим текст - много раз забегает в строчку 92 на этой проверке. Альтернативы?
                    result.Add(titles.ElementAt(i));                  
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Unable to get data from the site");
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
                Poem.Author =String.Join("", await GetElement(_sellectorAuthor));
                Poem.Title = String.Join("", await GetElement(_sellectorTitle));
                Poem.Text = String.Join("\n\n", await GetElement(_sellectorText));    
            }
            catch { throw; }
        }
        
        /// <summary>
        /// Заполняет список категориями стихотворения
        /// </summary>
        /// <returns></returns>
        private async Task SetCategory()
        {
            try { LCategory = await GetElement(_sellectorCategory); }
            catch { throw; }
        }
    }
}