using Carsale.Security.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carsale.Web.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeUserRoleAttribute : Attribute
    {
        /// <summary>
        /// The list of <see cref="UserOperation"/> values allowed to access this controller action.
        /// </summary>
        public UserRole[] UserRoles { get; private set; }
        /// <summary>
        /// Defines the <see cref="UserOperation"/> values that allow access to the controller action.
        /// </summary>
        /// <param name="userOperations">The list of <see cref="UserOperation"/> values allowed to access this controller action.</param>
        /// <exception cref="System.ArgumentException">UserOperation is null or empty.</exception>
        public AuthorizeUserRoleAttribute(params UserRole[] userRole)
        {
            if (userRole == null || userRole.Length == 0)
                throw new ArgumentException("UserRole is null or empty.");
            UserRoles = userRole;
        }
    }
}