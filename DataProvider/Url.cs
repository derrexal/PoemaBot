using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemaBotTelegram.DataProvider
{
    public class Url // модель таблицы в БД
    {
        private int id { get;  } // AutoIncrement
        public string url { get; set; }
        public string status { get; set; } = "None";
    }
}
