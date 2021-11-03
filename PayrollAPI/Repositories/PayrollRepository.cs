using PayrollAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollAPI.Repositories
{
    public class PayrollRepository : IPayrollRepository
    {
        private PayrollContext _context;

        public PayrollRepository(PayrollContext context)
        {
            _context = context;
        }

        public List<DBJobGroup> GetJobGroups()
        {
            return _context.JobGroup.ToList();

        }

        public DBTimeReport GetTimeReportById(int file_id)
        {
            DBTimeReport data = null;
            data = (from a in _context.TimeReport where a.report_id == file_id select a).FirstOrDefault();
            return data;
        }

        public List<DBTimeReportLog> GetTimeReportLogs()
        {
            List<DBTimeReportLog> report_logs = new List<DBTimeReportLog>();
            var timekeeping = (from a in _context.TimeReport
                               join b in _context.TimeReportLog
                               on a.report_id equals b.report_id
                               select new { report = a, report_log = b }).ToList();
            if (timekeeping.Count() > 0)
            {
                report_logs = (from a in timekeeping select a.report_log).ToList();
            }
            return report_logs;
        }

        public int InsertTimeReportLog(DBTimeReport time_report, List<DBTimeReportLog> time_report_log)
        {
            int insert = 0;
            _context.Database.BeginTransaction();
            try
            {
                if (time_report != null && time_report_log.Count > 0)
                {
                    _context.TimeReport.Add(time_report);
                    _context.TimeReportLog.BulkInsert(time_report_log);
                    insert = _context.SaveChanges();
                }
                _context.Database.CommitTransaction();

            }
            catch(Exception ex)
            {
                insert = 0;
                _context.Database.RollbackTransaction();
                throw ex;
            }
            
            return insert;
        }


        /* public List<DBTimeReportLog> GetReportLog()
         {
             return (from a in _context.TimeReportLog select a).ToList();
         }

         public List<DBTimeReportLog> GetReportLogById(int file_id)
         {
             return (from a in _context.TimeReportLog where a.report_id == file_id select a).ToList();
         }*/
    }
}
