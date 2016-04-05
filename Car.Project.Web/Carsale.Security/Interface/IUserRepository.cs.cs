using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carsale.Security.Interface
{
    public interface IUserRepository
    {
        /// <summary>
        /// Validate if the username and password are correct
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="password">Password</param>        
        /// <param name="filePath">full file path</param>
        /// <returns>True if the password and username is correct. Otherwise, false</returns>
        IUserDetails GetUserDetails(string userName, string filePath);
        /// <summary>
        /// Get the user by user name, the user name (email) is unique
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="filePath">full file path</param>
        /// <returns>User details</returns>
        bool ValidateUser(string userName, string password, string filePath);
    }
}
