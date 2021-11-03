using PayrollAPI.Models;
using PayrollAPI.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PayrollAPI
{
    public class Helper
    {
        public static string GetFileExtension(string filename)
        {
            return Path.GetExtension(filename);
        }

        public static string GetFileNameWithoutExtension(string filename)
        {
            return Path.GetFileNameWithoutExtension(filename);
        }

        public static bool IsDataExists(DataTable dt)
        {
            return dt != null && dt.Rows.Count > 0;
        }

        public static List<EmployeeReport> GenerateReport(DataTable time_log, DataTable rate_log)
        {
            List<EmployeeReport> report = new List<EmployeeReport>();
           
            if (time_log.Rows.Count > 0)
            {
                foreach (DataRow row in time_log.Rows)
                {
                    DateTime date = Convert.ToDateTime(row["date"].ToString());
                    string employee_id = row["employee_id"].ToString();
                    string start_date = string.Empty;
                    string end_date = string.Empty;
                    string employee_report_key = string.Empty;
                    if (date.Day <= 15)
                    {
                        employee_report_key = string.Concat("FH_", employee_id, date.Month, date.Year);
                        start_date = new DateTime(date.Year, date.Month, 1).ToString(Constant.report_date_format);
                        end_date = new DateTime(date.Year, date.Month, 15).ToString(Constant.report_date_format);
                    }
                    else
                    {
                        employee_report_key = string.Concat("SH_", employee_id, date.Month, date.Year);
                        start_date = new DateTime(date.Year, date.Month, 16).ToString(Constant.report_date_format);
                        end_date = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year,date.Month)).ToString(Constant.report_date_format);
                    }
                    float amount = 0;
                    var group_row = rate_log.AsEnumerable().Where(a => a.Field<string>("group").Equals(row["job_group"].ToString())).FirstOrDefault();
                    if (row["hours_worked"] != null && group_row["rate"] != null)
                    {
                        amount = float.Parse(row["hours_worked"].ToString()) * float.Parse(group_row["rate"].ToString());
                    }

                    var report_exist = (from a in report where a.key == employee_report_key select a).FirstOrDefault();
                    if (report_exist != null)
                    {
                        string old_amount = report_exist.amount_paid_text;
                        float old_value = 0;
                        float.TryParse(old_amount.Replace("$", string.Empty), out old_value);
                        report_exist.amount_paid_text = string.Format(Constant.report_amount_format, old_value + amount);

                    }
                    else
                    {
                        report.Add(new EmployeeReport
                        {
                            key = employee_report_key,
                            employee_id = employee_id,
                            pay_period = new PayPeriod { start_date = start_date, end_date = end_date },
                            amount_paid_text = string.Format(Constant.report_amount_format, amount)

                        });
                    }
                    

                }
            }
            var report_order = (from a in report orderby a.employee_id, a.pay_period.start_date ascending select a).ToList();                   
            return report_order;
        }

        public static DataTable ConvertCSVToDatable(string path)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(path))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }


            return dt;
        }

        public static DataTable CreateTableFromFile(string file_content)
        {
            DataTable dt = new DataTable();
            return dt;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
