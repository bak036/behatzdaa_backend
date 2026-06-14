using Nofshonit.Infrastructure.Utils.IOC;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Services.Base
{
    public class BaseService
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
