using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PayrollAPI.Models
{
    public class EmployeeReport
    {
        
        [JsonIgnore]
        public string key { get; set; }

        [JsonPropertyName("employeeId")]
        public string employee_id { get; set; }

        [JsonPropertyName("payPeriod")]
        public PayPeriod pay_period { get; set; }
        
        [JsonPropertyName("amountPaid")]
        public string amount_paid_text { get; set; }
    }

    public class PayPeriod
    {
        [JsonPropertyName("startDate")]
        public string start_date { get; set; }

        [JsonPropertyName("endDate")]
        public string end_date { get; set; }

    }



}
