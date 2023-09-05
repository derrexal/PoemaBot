using Microsoft.Extensions.Logging;
using NLog;
using PoemaBotTelegram.SQL;
using System;
using System.Net;
using System.Net.Http.Headers;

namespace PoemaBotTelegram
{
    public class UrlApp
    {
        private const string _baseUrl = "https://www.culture.ru/poems/"; // начало ссылки для парсинга стихов (в файле url_list.txt (в базе) хранятся строки с продолжением этой ссылки)
        private DB _db { get; }
        private Logger _logger { get; }

        public UrlApp()
        {
            _db = new DB();
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Определяет ссылку после редиректа
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetRedirectUrl(uint num)
        {
            var url = _baseUrl + num;
            string newUrl = url;
            HttpWebRequest req = null;
            HttpWebResponse resp = null;
            
            try
            {
                req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "HEAD";
                req.AllowAutoRedirect = false;
                resp = (HttpWebResponse)req.GetResponse();
                if (resp.StatusCode == HttpStatusCode.OK) return newUrl;
                else if (resp.StatusCode == HttpStatusCode.NotFound) return null;
                else if (resp.StatusCode == HttpStatusCode.PermanentRedirect)
                {
                    newUrl = resp.Headers["Location"];
                    if (newUrl == null)
                        return url;
                    if (newUrl == "/poems/") return null;
                    if (newUrl.IndexOf("://", System.StringComparison.Ordinal) == -1)
                    {
                        // Doesn't have a URL Schema, meaning it's a relative or absolute URL
                        Uri u = new Uri(new Uri(url), newUrl);
                        newUrl = u.ToString();
                    }
                }
            }
            catch (WebException)
            {
                // Return the last known good URL
                return newUrl;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (resp != null)
                    resp.Close();
            }
            return newUrl;
        }

        public async Task GetAllUrlFromRedirect()
        {
            // здесь определяем начальное значение
            try
            { 
                for (uint num = 50000; num < Settings.MAX_NUM_POEMAS; num++)
                {
                    // Получем ссылку после редиректа
                    var uri = GetRedirectUrl(num);
                    if (uri == null) continue;
                    // устанавливаем поля провайдеру
                    var url = new DataProvider.Url();
                    url.Status = Constants.statusNone;
                    url.url = uri;
                    // Записываем в базу
                    if (_db.urlTable.CheckUrl(url) == false) continue; // если такая ссылка уже есть - идём к следующей ссылке
                    await _db.urlTable.AddUrlAsync(url); 
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Unable to get REDIRECT URL for the site");
                throw;
            }
        }

    }
}
