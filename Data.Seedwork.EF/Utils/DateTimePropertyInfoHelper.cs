﻿using Infrastructure.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.EF.Utils
{
    public static class DateTimePropertyInfoHelper
    {
        
        private static readonly ConcurrentDictionary<Type, EntityDateTimePropertiesInfo> DateTimeProperties;

        static DateTimePropertyInfoHelper()
        {
            DateTimeProperties = new ConcurrentDictionary<Type, EntityDateTimePropertiesInfo>();
        }

        public static EntityDateTimePropertiesInfo GetDatePropertyInfos(Type entityType)
        {
            return DateTimeProperties.GetOrAdd(
                       entityType,
                       key => FindDatePropertyInfosForType(entityType)
                   );
        }

        public static void NormalizeDatePropertyKinds(object entity, Type entityType)
        {
            var dateTimePropertyInfos = GetDatePropertyInfos(entityType);

            dateTimePropertyInfos.DateTimePropertyInfos.ForEach(property =>
            {
                var dateTime = (DateTime?)property.GetValue(entity);
                if (dateTime.HasValue)
                {
                    property.SetValue(entity, dateTime.Value);
                }
            });

            dateTimePropertyInfos.ComplexTypePropertyPaths.ForEach(propertPath =>
            {
                var dateTime = (DateTime?)ReflectionHelper.GetValueByPath(entity, entityType, propertPath);
                if (dateTime.HasValue)
                {
                    ReflectionHelper.SetValueByPath(entity, entityType, propertPath, dateTime.Value);
                }
            });
        }

        private static EntityDateTimePropertiesInfo FindDatePropertyInfosForType(Type entityType)
        {
            var datetimeProperties = entityType.GetProperties()
                                     .Where(property =>
                                         property.PropertyType == typeof(DateTime) ||
                                         property.PropertyType == typeof(DateTime?)
                                     ).ToList();

            var complexTypeProperties = entityType.GetProperties()
                                                   .Where(p => p.PropertyType.IsDefined(typeof(ComplexTypeAttribute), true))
                                                   .ToList();

            var complexTypeDateTimePropertyPaths = new List<string>();
            foreach (var complexTypeProperty in complexTypeProperties)
            {
                AddComplexTypeDateTimePropertyPaths(entityType.FullName + "." + complexTypeProperty.Name, complexTypeProperty, complexTypeDateTimePropertyPaths);
            }

            return new EntityDateTimePropertiesInfo
            {
                DateTimePropertyInfos = datetimeProperties,
                ComplexTypePropertyPaths = complexTypeDateTimePropertyPaths
            };
        }

        private static void AddComplexTypeDateTimePropertyPaths(string pathPrefix, PropertyInfo complexProperty, List<string> complexTypeDateTimePropertyPaths)
        {
            if (!complexProperty.PropertyType.IsDefined(typeof(ComplexTypeAttribute), true))
            {
                return;
            }

            var complexTypeDateProperties = complexProperty.PropertyType
                                                            .GetProperties()
                                                            .Where(property =>
                                                                property.PropertyType == typeof(DateTime) ||
                                                                property.PropertyType == typeof(DateTime?)
                                                            ).Select(p => pathPrefix + "." + p.Name).ToList();

            complexTypeDateTimePropertyPaths.AddRange(complexTypeDateProperties);

            var complexTypeProperties = complexProperty.PropertyType.GetProperties()
                                                  .Where(p => p.PropertyType.IsDefined(typeof(ComplexTypeAttribute), true))
                                                  .ToList();

            if (!complexTypeProperties.Any())
            {
                return;
            }

            foreach (var complexTypeProperty in complexTypeProperties)
            {
                AddComplexTypeDateTimePropertyPaths(pathPrefix + "." + complexTypeProperty.Name, complexTypeProperty, complexTypeDateTimePropertyPaths);
            }
        }
    }
}
