using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carsale.Data.Interface
{
   public  interface IEnquiryDetails
    {
        string Name { get; set; }
        string CarId { get; set; }
        string Email { get; set; }
        string ContactPhone { get; set; }
        string Comment { get; set; }
    }
}
