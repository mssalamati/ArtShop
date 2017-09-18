using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enitities
{
    public class SettingValue
    {
        [Key]
        public int Id { get; set; }
        public decimal IRRialRate { get; set; }
        public decimal EURate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdaterUser { get; set; }
    }
}
