using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PayrollAPI.Models;
using PayrollAPI.Repositories;
using PayrollAPI.Utility;

namespace PayrollAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PayrollController : ControllerBase
    {
        private IPayrollRepository payrollQuery;

        public PayrollController(IPayrollRepository repository)
        {
            payrollQuery = repository;
        }


        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public IActionResult PushTimeKeeping(IFormFile input_file)
        {
            var _responseMessage = new ResponseMessage();
            string save_path = string.Empty;
            string file_save_time = string.Empty;

            if (input_file == null)
            {
                _responseMessage.MessageStatus = "error";
                _responseMessage.MessageText = "File not found";
                return BadRequest(_responseMessage);//400
            }
            else
            { 
                string file_name = input_file.FileName;
                if (!Helper.GetFileExtension(file_name).ToLower().Equals(".csv"))
                {
                    _responseMessage.MessageStatus = "error";
                    _responseMessage.MessageText = "Invalid extension of the file, please try again with csv format.";
                    return BadRequest(_responseMessage);//400
                }
                string file_id = Helper.GetFileNameWithoutExtension(file_name).Split('-')[2];
                if (!string.IsNullOrEmpty(file_id))
                {
                    int file_id_int = Int32.Parse(file_id);
                    var current_file = payrollQuery.GetTimeReportById(file_id_int);
                    if (current_file != null)
                    {
                        _responseMessage.MessageStatus = "error";
                        _responseMessage.MessageText = string.Format("File information already exists for the ID '{0}', please upload file with another ID",file_id_int);
                        return StatusCode(StatusCodes.Status422UnprocessableEntity, _responseMessage);
                    }
                    //temp saving the file in local
                    save_path = Path.Combine(Directory.GetCurrentDirectory(), "Temp", file_name);
                    using (var filestream = new FileStream(save_path, FileMode.Create))
                    {
                        input_file.CopyToAsync(filestream);
                    }

                    DataTable time_log_data = Helper.ConvertCSVToDatable(save_path);

                    int state = 0;
                    try
                    {
                        //storing the time and file info
                     
                        List<DBTimeReportLog> time_log = new List<DBTimeReportLog>();
                        foreach (DataRow row in time_log_data.Rows)
                        {
                            //knowing the format is correct
                            string[] date_split = (row["date"].ToString()).Split("/");
                            DateTime report_date = new DateTime(int.Parse(date_split[2]), int.Parse(date_split[1]), int.Parse(date_split[0]));

                            //preparing the time log list
                            time_log.Add(new DBTimeReportLog {
                                date = report_date,
                                hours_worked = float.Parse(row["hours worked"].ToString()),
                                employee_id = int.Parse(row["employee id"].ToString()),
                                job_group = char.Parse(row["job group"].ToString()),
                                report_id = file_id_int
                            });
                  
                        }
                        state = payrollQuery.InsertTimeReportLog(
                            new DBTimeReport { report_id = file_id_int, report_date = DateTime.Now },
                            time_log
                            );

                        file_save_time = DateTime.Now.ToString("dddd, dd MMMM yyyy h:mm tt"); //for display purposeo only
                    }
                    catch (Exception ex)
                    {
                        _responseMessage.MessageStatus = "error";
                        _responseMessage.MessageText = string.Concat("Internal Server Error: ", ex.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, _responseMessage);
                    }
                    finally
                    {   //delete the temp file in all cases to avoid extra space
                        System.IO.File.Delete(save_path);
                    }

                    _responseMessage.MessageStatus = "success";
                    _responseMessage.MessageText = string.Format("File content saved succesfully with ID '{0}' on {1}", file_id, file_save_time);
                }

            }

            return Ok(_responseMessage);

        }

        [HttpGet("report")]
        public IActionResult GetPayrollReport()
        {
            
            var _responseMessage = new ResponseMessage();
            try
            {
               
                List<DBTimeReportLog> time_log = payrollQuery.GetTimeReportLogs();
                List<DBJobGroup> rates = payrollQuery.GetJobGroups();

                DataTable log_table = Helper.ToDataTable(time_log);
                DataTable rate_table = Helper.ToDataTable(rates);

                if (Helper.IsDataExists(log_table))
                {
                    PayrollReport report = new PayrollReport();
                    report.employee_reports = Helper.GenerateReport(log_table, rate_table);
                    PayrollReportResponse report_response = new PayrollReportResponse
                    {
                        payrollreport = report
                     };
                   
                    return Ok(report_response);
                }
                else
                {
                    _responseMessage.MessageStatus = "error";
                    _responseMessage.MessageText = "No record found, please upload a file or try again later.";
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, _responseMessage);
                }

            }
            catch (Exception ex)
            {
                _responseMessage.MessageStatus = "error";
                _responseMessage.MessageText = string.Concat("Internal Server Error: ", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _responseMessage);
            } 
        }
    }
}