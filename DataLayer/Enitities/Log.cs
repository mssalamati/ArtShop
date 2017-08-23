using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class Log
    {
        public virtual int Id { get; set; }
        public string Location { get; set; }
        public string Message { get; set; }
        public DateTime date { get; set; }
        public int Type { get; set; }
        public Log()
        {
            date = DateTime.Now;
        }
    }
}
