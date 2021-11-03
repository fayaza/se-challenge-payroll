using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PayrollAPI.Models
{
    
    public class PayrollReport
    {

        [JsonPropertyName("employeeReports")]
        public List<EmployeeReport> employee_reports { get; set; }
    }

    public class PayrollReportResponse
    {
        [JsonPropertyName("payrollReport")]

        public PayrollReport payrollreport { get; set; }
    }
}
