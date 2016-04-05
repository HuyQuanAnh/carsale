using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carsale.Web.Models
{
    public class LoginDetailsModel
    {
        public string Password { get;set;}
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
    }
}