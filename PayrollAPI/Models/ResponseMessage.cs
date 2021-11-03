using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PayrollAPI.Models
{
    public class ResponseMessage
    {
        [JsonPropertyName("status_code")]
        public string MessageStatus { get; set; }

        [JsonPropertyName("status")]
        public string MessageText { get; set; }

        
    }
}
