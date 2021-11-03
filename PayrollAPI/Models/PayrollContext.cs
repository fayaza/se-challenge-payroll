using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PayrollAPI.Models
{
    public partial class PayrollContext : DbContext
    {
        public PayrollContext()
        {
        }
        public PayrollContext(DbContextOptions<PayrollContext> opt)
            : base(opt)
        {
        }
        
        public DbSet<DBTimeReport> TimeReport { get; set; }
        public DbSet<DBTimeReportLog> TimeReportLog { get; set; }
        public DbSet<DBJobGroup> JobGroup { get; set; }

    }
}
