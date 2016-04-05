
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carsale.Web.Models
{
    public class ResponseDetails
    {
        public bool IsSuccessful {get;set;}
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public ResponseDetails()
        {
            IsSuccessful = true;
        }
    }
}