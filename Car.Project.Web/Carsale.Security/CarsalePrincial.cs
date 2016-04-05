using Carsale.Security.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Carsale.Security
{
    public class CarsalePrincial : IPrincipal
    {

        public CarsalePrincial(IIdentity identity)
        {
            Identity = identity;
        }

        public IIdentity Identity
        {
            get;
            private set;
        }

        public UserDetails UserDetails { get; set; }

        public bool IsInRole(string role)
        {
            return string.Equals(UserDetails.UserRole.ToString(), role, StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
