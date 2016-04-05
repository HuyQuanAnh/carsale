using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carsale.Data.Interface
{
    public class ICarDetails
    {
        string Id { get; set; }
        string Photo { get; set; }
        string Make { get; set; }
        string Model { get; set; }
        int? Year { get; set; }
        decimal? Price { get; set; }
        string DealerEmail { get; set; }
        string DealerContact { get; set; }
        string Phone { get; set; }
        string DealerABN { get; set; }
        string Description { get; set; }

    }
}
