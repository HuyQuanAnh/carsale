using Carsale.Security.Interface;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Carsale.Security
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Validate if the username and password are correct
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="password">Password</param>        
        /// <param name="filePath">full file path</param>
        /// <returns>True if the password and username is correct. Otherwise, false</returns>
        public bool ValidateUser(string userName, string password, string filePath)
        {
            try
            {
                var users = GetUserList(filePath);
                return users.Any(u => string.Equals(u.UserName, userName, StringComparison.CurrentCultureIgnoreCase) && string.Equals(u.Password, password, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(string.Format("Failed to validate user {0}", userName), ex);
                return false;
            }
        }
        /// <summary>
        /// Get the user by user name, the user name (email) is unique
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="filePath">full file path</param>
        /// <returns>User details</returns>
        public IUserDetails GetUserDetails(string userName, string filePath)
        {
            try
            {
                var users = GetUserList(filePath);
                return users.FirstOrDefault(u => string.Equals(u.UserName, userName, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception ex)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(string.Format("Failed to get user {0}", userName), ex);
                return null;
            }
        }
        /// <summary>
        /// Get list of user from XML file
        /// </summary>
        /// <param name="filePath">full file path</param>
        /// <returns>List of users</returns>
        private List<UserDetails> GetUserList(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                List<UserDetails> users = (List<UserDetails>)serializer.Deserialize(file, typeof(List<UserDetails>));
                return users ?? new List<UserDetails>();
            }
        }
    }
}
