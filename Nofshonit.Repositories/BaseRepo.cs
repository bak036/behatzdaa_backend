using Nofshonit.Common.Infrastructure;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;
using Nofshonit.Infrastructure.Utils.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Repositories
{
    public class BaseRepo
    {
        public ICustomContainer Container
        {
            get
            {
                return ContainerManager.Container;
            }
        }

        public ICacheManager CacheManager
        {
            get
            {
                return Container.Resolve<ICacheManager>();
            }
        }

        public IContextManager ContextManager
        {
            get
            {
                return Container.Resolve<IContextManager>();
            }
        }

        public ILog LogManager
        {
            get
            {
                return Container.Resolve<ILog>();
            }
        }
    }
}
