using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Seedwork.ObjectMapper;
using Infrastructure.Dependency;

namespace Application.ObjectMapper.AutoMapper
{
    public class AutoMapperObjectMapper : IObjectMapper, ISingletonDependency
    {
        public TDestination Map<TDestination>(object source)
        {
            return source.MapTo<TDestination>();
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return source.MapTo(destination);
        }
    }
}
