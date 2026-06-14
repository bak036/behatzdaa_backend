using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nofshonit.Infrastructure.Utils.IOC
{
    public interface ICustomContainer
    {
        TService Resolve<TService>();

        TService ResolveByOrganization<TService>(string organizationId);

        bool IsRegistered<TService>();
    }
}
