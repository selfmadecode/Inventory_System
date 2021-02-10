using inventoryAppWebUi.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Repository;
using inventoryAppDomain.Services;

namespace inventoryAppWebUi.Infrastructures
{
    public class NinjectDependenceResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependenceResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind(typeof(ApplicationDbContext)).ToSelf();
            kernel.Bind<IRoleService>().To<RoleService>();
        }
    }
}