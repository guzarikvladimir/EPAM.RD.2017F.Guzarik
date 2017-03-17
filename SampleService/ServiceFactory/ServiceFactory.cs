using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CustomServices.Abstract;
using CustomServices.Concrete;

namespace ServiceFactory
{
    public static class ServiceFactory
    {
        public static IService<User> Create(AppDomain domain, string domainName, string assemblyString, Type serviceType)
        {
            AppDomainSetup ads = new AppDomainSetup
            {
                ApplicationBase = domain.BaseDirectory,
                PrivateBinPath = Path.Combine(domain.BaseDirectory, domainName),
                ConfigurationFile = domain.SetupInformation.ConfigurationFile
            };

            var newDomain = AppDomain.CreateDomain(domainName, null, ads);

            var assembly = Assembly.Load(assemblyString);

            var instance = newDomain.CreateInstanceAndUnwrap(assembly.FullName, serviceType.FullName) as IService<User>;

            return instance;
        }
    }
}
