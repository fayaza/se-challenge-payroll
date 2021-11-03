using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollAPI.Models
{
    [Table("time_report")]
    public class DBTimeReport
    {
        public int id { get; set; }
        public int report_id { get; set; }
        public DateTime report_date { get; set; }
       // internal PayrollContext _context { get; set; }

        /*public DBTimeReport(PayrollContext db)
        {
            _context = db;
        }*/
    }
}
