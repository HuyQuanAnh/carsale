using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carsale.Data.Interface
{
    interface ILoginDetails
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}
