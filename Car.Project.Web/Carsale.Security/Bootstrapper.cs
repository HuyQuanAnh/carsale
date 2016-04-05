using Carsale.Security.Interface;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carsale.Security
{
    public static class Bootstrapper
    {
        /// <summary>
        /// Registers this library's interfaces and classes in the <see cref="IUnityContainer"/>.
        /// </summary>
        /// <param name="container">The container to populate.</param>
        public static void BuildUnityContainer(IUnityContainer container)
        {
            container.RegisterType<IUserRepository, UserRepository>();
        }
    }
}
