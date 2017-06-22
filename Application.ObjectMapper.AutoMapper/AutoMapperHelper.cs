using AutoMapper;
using Infrastructure.Collections.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.ObjectMapper.AutoMapper
{
    internal static class AutoMapperHelper
    {
        public static void CreateMap(Type type)
        {
            CreateMap<AutoMapFromAttribute>(type);
            CreateMap<AutoMapToAttribute>(type);
            CreateMap<AutoMapAttribute>(type);
        }

        public static void CreateMap<TAttribute>(Type type)
            where TAttribute : AutoMapAttribute
        {
            if (!type.IsDefined(typeof(TAttribute)))
            {
                return;
            }
            var mapProfiles = type.GetCustomAttribute<MapProfileAttribute>();
            foreach (var autoMapToAttribute in type.GetCustomAttributes<TAttribute>())
            {
                if (autoMapToAttribute.TargetTypes.IsNullOrEmpty())
                {
                    continue;
                }

                foreach (var targetType in autoMapToAttribute.TargetTypes)
                {
                    if (autoMapToAttribute.Direction.HasFlag(AutoMapDirection.To))
                    {
                        CreateToMap(targetType, type, mapProfiles);
                        //Mapper.CreateMap(type, targetType);
                    }

                    if (autoMapToAttribute.Direction.HasFlag(AutoMapDirection.From))
                    {
                        CreateFromMap(targetType, type, mapProfiles);
                        //Mapper.CreateMap(targetType, type);
                    }
                }
            }
        }
        private static void CreateToMap(Type sourceType, Type destinationType, MapProfileAttribute profileAttribute)
        {
            if (profileAttribute != null && GetProfileType(sourceType, destinationType, profileAttribute.ProviderTypes) != null)
            {
                try
                {
                    dynamic profile = Activator.CreateInstance(GetProfileType(sourceType, destinationType, profileAttribute.ProviderTypes));
                    profile.MapTo();

                    Type mapType = typeof(Mapper);
                    var mapMethods = mapType.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.Name == "CreateMap" && m.IsGenericMethod && !m.GetParameters().Any()).ToList();
                    var method = mapMethods[0].MakeGenericMethod(destinationType, sourceType);
                    dynamic expression = method.Invoke(null, null);
                    IList memberProfiles = profile.DestinationToSource;
                    foreach (dynamic memberProfile in memberProfiles)
                    {
                        expression.ForMember(memberProfile.DestinationMember, (Action<dynamic>)((map) => map.MapFrom(memberProfile.SourceMember)));
                    }
                }
                catch
                {
                    Mapper.CreateMap(destinationType, sourceType);
                }
                
            }
            else
            {
                Mapper.CreateMap(destinationType, sourceType);
            }
        }
        private static void CreateFromMap(Type sourceType,Type destinationType, MapProfileAttribute profileAttribute)
        {
            if(profileAttribute!=null && GetProfileType(sourceType,destinationType,profileAttribute.ProviderTypes)!=null)
            {
                try
                {
                    dynamic profile = Activator.CreateInstance(GetProfileType(sourceType, destinationType, profileAttribute.ProviderTypes));
                    profile.MapFrom();

                    Type mapType = typeof(Mapper);
                    var mapMethods = mapType.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.Name == "CreateMap" && m.IsGenericMethod && !m.GetParameters().Any()).ToList();
                    var method = mapMethods[0].MakeGenericMethod(sourceType, destinationType);
                    dynamic expression = method.Invoke(null, null);
                    IList memberProfiles = profile.SourceToDestination;
                    foreach (dynamic memberProfile in memberProfiles)
                    {
                        expression.ForMember(memberProfile.DestinationMember, (Action<dynamic>)((map) => map.MapFrom(memberProfile.SourceMember)));
                    }
                }
                catch
                {
                    Mapper.CreateMap(sourceType, destinationType);
                }
                
            }
            else
            {
                Mapper.CreateMap(sourceType, destinationType);
            }
        }
        private static Type GetProfileType(Type sourceType,Type destinationType,Type[] profiles)
        {
            return profiles.FirstOrDefault(m => m.BaseType.IsGenericType && 
                                         m.BaseType.GenericTypeArguments.Length == 2 && 
                                         m.BaseType.GenericTypeArguments[0].Equals(sourceType) && 
                                         m.BaseType.GenericTypeArguments[1].Equals(destinationType));
        }
    }
}
