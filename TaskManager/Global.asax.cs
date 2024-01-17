using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TaskManager.Core.Repositories;
using TaskManager.Implementation;

namespace TaskManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ITaskRepository TaskRepository = new TaskRepository();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine
            {
                PartialViewLocationFormats = new string[]
                {
                    "~/Web/Views/{1}/{0}.cshtml",
                    "~/Web/Views/Shared/{0}.cshtml"
                },
                ViewLocationFormats = new string[]
                {
                    "~/Web/Views/{1}/{0}.cshtml",
                    "~/Web/Views/Shared/{0}.cshtml"
                },
            });

            DependencyResolver.SetResolver(new TaskDI());
        }

        private class TaskDI : IDependencyResolver
        {

            public object GetService(Type serviceType)
            {
                return serviceType == typeof(ITaskRepository) ? TaskRepository : null;
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return serviceType == typeof(ITaskRepository) ? new List<object> { TaskRepository } : new List<object>();
            }
        }
    }
}
