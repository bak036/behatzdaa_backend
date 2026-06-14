using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Infrastructure.Utils.IOC
{
    public static class ContainerManager
    {
        public static ICustomContainer Container = new IocContainer();
    }
}
