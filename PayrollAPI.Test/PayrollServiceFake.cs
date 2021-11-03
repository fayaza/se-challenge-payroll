using PayrollAPI.Controllers;
using PayrollAPI.Models;
using PayrollAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayrollAPI.Test
{
    public class PayrollServiceFake : IPayrollRepository
    {
        private readonly List<DBJobGroup> _jobGroup;
        private readonly List<DBTimeReport> _timeReport;
        private readonly List<DBTimeReportLog> _timeReportLog;

        public PayrollServiceFake()
        {
            _jobGroup = new List<DBJobGroup>()
            {
                new DBJobGroup() {id=1, group='A', rate =20},
                new DBJobGroup() {id=2, group='B', rate=30}
            };

            _timeReport = new List<DBTimeReport>();
            _timeReport.Add(new DBTimeReport { id = 1, report_date = DateTime.Now, report_id = 999 });
            
            _timeReportLog = new List<DBTimeReportLog>()
            {
                new DBTimeReportLog() {id = 1, report_id = 999, date = new DateTime(2020,1,4), employee_id = 1, hours_worked = 10, job_group = 'A' },
                new DBTimeReportLog() {id = 1, report_id = 999, date = new DateTime(2020,1,4), employee_id = 1, hours_worked = 5, job_group = 'A' },
                new DBTimeReportLog() {id = 1, report_id = 999, date = new DateTime(2020,1,20), employee_id = 2, hours_worked = 3, job_group = 'B' },
                new DBTimeReportLog() {id = 1, report_id = 999, date = new DateTime(2020,1,20), employee_id = 1, hours_worked = 4, job_group = 'A' },
            };
        }
        public List<DBJobGroup> GetJobGroups()
        {
            return _jobGroup;
        }

        public DBTimeReport GetTimeReportById(int file_id)
        {

            return _timeReport.Find(a => a.report_id == file_id);

        }

        public List<DBTimeReportLog> GetTimeReportLogs()
        {
            return _timeReportLog;
        }

        public int InsertTimeReportLog(DBTimeReport time_report, List<DBTimeReportLog> time_report_log)
        {
            throw new NotImplementedException();
        }

     
    }
}
