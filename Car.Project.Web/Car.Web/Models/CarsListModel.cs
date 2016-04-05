using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carsale.Web.Models
{
    public class CarsListModel
    {
        public CarsListModel()
        {
            Cars = new List<CarDetailsModel>();
        }
        public List<CarDetailsModel> Cars { get; set; }

    }
}