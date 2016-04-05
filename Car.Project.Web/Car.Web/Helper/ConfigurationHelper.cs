using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Carsale.Web.Helper
{
    public static class ConfigurationHelper 
    {
        
        public static bool EnableNotification
        {
            get
            {
                var enableNotificationStr = ConfigurationManager.AppSettings["EnableNotification"];
                bool enableNotification = false;
                bool.TryParse(enableNotificationStr,out enableNotification);
                return enableNotification;
            }
        }
    }
}
