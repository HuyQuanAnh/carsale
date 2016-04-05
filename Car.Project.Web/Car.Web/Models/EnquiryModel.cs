using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carsale.Web.Models
{
    public class EnquiryModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public string Comment { get; set; }
        public string CarId { get; set; }
    }
}