using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemaBotTelegram.Interface
{
    internal interface ITable
    {
        private static int _countRecord; // Общее количество записей в таблице

        internal static int CountRecord
        {
            get { return _countRecord; }
        }

        internal int GetCountRecord();
    }
}
