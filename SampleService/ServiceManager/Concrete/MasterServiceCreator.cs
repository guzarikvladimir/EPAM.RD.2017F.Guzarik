using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CustomServices.Abstract;
using CustomServices.Concrete;

namespace ServiceManager.Concrete
{
    public class MasterServiceCreator
    {
        public UserServiceMaster Create(string domainName)
        {
            var appDomainSetup = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, domainName)
            };
            AppDomain domain = AppDomain.CreateDomain(domainName, null, appDomainSetup);
            var type = domain.CreateInstanceAndUnwrap(Assembly.Load("CustomServices").FullName, typeof(UserServiceMaster).FullName) as UserServiceMaster;
            return type;
        }
    }
}
