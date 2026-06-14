using Nofshonit.Infrastructure.Utils.IOC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Infrastructure
{
    public class BaseInfrastructure
    {
        public ICustomContainer Container
        {
            get
            {
                return ContainerManager.Container;
            }
        }
    }
}
