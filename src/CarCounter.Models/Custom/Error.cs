using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCounter.Models
{
    public class Error
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public override string ToString() =>
            System.Text.Json.JsonSerializer.Serialize(this);
    }
}
