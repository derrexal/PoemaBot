using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemaBotTelegram.DataProvider
{
    public class Category // модель таблицы в БД
    {
        private int Id { get;} // AutoIncrement
        public string Name { get; set; }
    }
}
    