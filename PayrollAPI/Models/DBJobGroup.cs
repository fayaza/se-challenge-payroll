using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollAPI.Models
{
    [Table("job_group")]
    public class DBJobGroup
    {
        public int id { get; set; }
        public char group { get; set; }
        public float rate { get; set; }
      
    }
}
