using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity.Mvc4;

namespace Carsale.Web
{
    public class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }        
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            Carsale.Security.Bootstrapper.BuildUnityContainer(container);
            Carsale.Data.Bootstrapper.BuildUnityContainer(container);
            return container;
        }

    }
}