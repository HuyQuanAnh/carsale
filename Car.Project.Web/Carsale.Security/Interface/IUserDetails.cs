using Carsale.Security.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carsale.Security.Interface
{
    public interface IUserDetails
    {
        string UserName { get; set; }
        string Password { get; set; }
        UserRole UserRole { get; set; }
    }
}
