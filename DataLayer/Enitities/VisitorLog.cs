using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class VisitorLog
    {
        public int Id { get; set; }
        public DateTime VisitedOn { get; set; }
        public string LocationIP { get; set; }
        public string BrowserName { get; set; }

        public VisitorLog()
        {
            VisitedOn = DateTime.Now;
        }
    }
}
