using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carsale.Web.Models
{
    public class CarDetailsModel
    {
        public string Id { get; set; }
        public string Photo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
        public decimal? Price { get; set; }
        public string DealerEmail { get; set; }
        public string DealerContact { get; set; }
        public string Phone { get; set; }
        public string DealerABN { get; set; }
        public string Description { get; set; }
        public ResponseDetails ResponseDetails { get; set; }
        
    }
}