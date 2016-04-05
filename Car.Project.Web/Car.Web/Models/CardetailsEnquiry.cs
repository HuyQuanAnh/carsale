using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carsale.Web.Models
{
    public class CardetailsEnquiry
    {
        public CardetailsEnquiry ()
        {
            CarDetailsModel = new CarDetailsModel();
            EnquiryModel = new EnquiryModel();
        }

        public CarDetailsModel CarDetailsModel { get; set; }
        public EnquiryModel EnquiryModel { get; set; }
        public ResponseDetails ResponseDetails { get; set; }
    }
}