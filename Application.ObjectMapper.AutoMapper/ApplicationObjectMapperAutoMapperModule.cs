using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Modules;
using Infrastructure.Configuration;
using Infrastructure.Reflection;
using Application.Seedwork.ObjectMapper;
using System.Reflection;

namespace Application.ObjectMapper.AutoMapper
{
    [DependsOn(typeof(KernelModule),typeof(Application.Seedwork.ApplicationSeedworkModule))]
    public class ApplicationObjectMapperAutoMapperModule : Infrastructure.Modules.Module
    {
        public ILogger Logger { get; set; }

        private readonly ITypeFinder _typeFinder;

        private static bool _createdMappingsBefore;
        private static readonly object _syncObj = new object();

        public ApplicationObjectMapperAutoMapperModule(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
            Logger = NullLogger.Instance;
        }
        public override void PreInitialize()
        {
            Configuration.ReplaceService<IObjectMapper, AutoMapperObjectMapper>(); 
        }
        public override void PostInitialize()
        {
            CreateMappings();
        }
        private void CreateMappings()
        {
            lock (_syncObj)
            {
                
                if (_createdMappingsBefore)
                {
                    return;
                }

                FindAndAutoMapTypes();
                _createdMappingsBefore = true;
            }
        }

        private void FindAndAutoMapTypes()
        {
            var types = _typeFinder.Find(type =>
                type.IsDefined(typeof(AutoMapAttribute)) ||
                type.IsDefined(typeof(AutoMapFromAttribute)) ||
                type.IsDefined(typeof(AutoMapToAttribute))
                );

            Logger.Debug(string.Format("找到 {0} 个类定义了 auto mapping 特性", types.Length));
            foreach (var type in types)
            {
                Logger.Debug(type.FullName);
                AutoMapperHelper.CreateMap(type);
            }
        }

        
    }
}
