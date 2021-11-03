using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollAPI.Models
{
    [Table("time_report_log")]
    public class DBTimeReportLog 
    {
        public int id { get; set; }
        public int report_id { get; set; }
        public DateTime date { get; set; }
        public float hours_worked { get; set; }
        public int employee_id { get; set; }
        public char job_group { get; set; }
 
    }
}
