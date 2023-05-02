using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoemaBotTelegram.Interface
{
    public interface IPars
    {
        //private Poema poema;

        public Task<string> GetElement();
        
    }
}
