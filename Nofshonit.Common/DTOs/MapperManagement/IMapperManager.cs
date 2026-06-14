using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nofshonit.Common.DTOs.MapperManagement
{
    public interface IMapperManager
    {
        dynamic Map(Object source, Type destType);

        void Init(IMapper mapper);
    }
}
