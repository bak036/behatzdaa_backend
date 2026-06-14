using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Nofshonit.Common;
using Nofshonit.Common.Infrastructure;
using Nofshonit.Infrastructure.Communication;
using Nofshonit.Infrastructure.Configuration;
using Nofshonit.Infrastructure.Utils.IOC;

namespace Nofshonit.BL
{


    public class BaseBL
    {

        public List<string> LocalMembersAllowedLogin
        {
            get
            {
                var members = Container.Resolve<IConfigurationManager>().GetConfigByValue<string>(ConfigurationKey.LocalMembersAllowedLogin);
                if (!string.IsNullOrWhiteSpace(members))
                    return new List<string>(members.Split(';'));
                return new List<string>();
            }
        }

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


    }
}
