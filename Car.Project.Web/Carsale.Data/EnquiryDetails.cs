using Carsale.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carsale.Data
{
    public class EnquiryDetails : IEnquiryDetails
    {
        public string Name { get; set; }
        public string CarId { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public string Comment { get; set; }
    }
}
