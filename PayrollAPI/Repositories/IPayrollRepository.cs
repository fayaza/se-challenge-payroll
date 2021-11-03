using PayrollAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayrollAPI.Repositories
{
    public interface IPayrollRepository
    {
        int InsertTimeReportLog(DBTimeReport time_report, List<DBTimeReportLog> time_report_log);
        DBTimeReport GetTimeReportById(int file_id);
        List<DBTimeReportLog> GetTimeReportLogs();
        List<DBJobGroup> GetJobGroups();
    }
}
