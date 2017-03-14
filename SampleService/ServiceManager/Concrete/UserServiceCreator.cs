using System;
using System.IO;
using System.Reflection;
using CustomServices.Concrete;

namespace ServiceManager.Concrete
{
    public class UserServiceCreator
    {
        public UserService Create(string domainName)
        {
            var appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, domainName)
            };
            AppDomain domain = AppDomain.CreateDomain(domainName, null, appDomainSetup);
            var type = domain.CreateInstanceAndUnwrap(Assembly.Load("CustomServices").FullName, typeof(UserService).FullName) as UserService;
            return type;
        }
    }
}
