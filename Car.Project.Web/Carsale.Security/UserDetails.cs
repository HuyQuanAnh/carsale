using Carsale.Security.Enum;
using Carsale.Security.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carsale.Security
{
    public class UserDetails : IUserDetails
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; }
    }
}
