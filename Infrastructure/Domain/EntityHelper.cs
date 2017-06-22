using Infrastructure.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public static class EntityHelper
    {
        public static bool IsEntity(Type type)
        {
            return ReflectionHelper.IsAssignableToGenericType(type, typeof(IEntity<>));
        }
        public static Type GetPrimaryKeyType<TEntity>()
        {
            return GetPrimaryKeyType(typeof(TEntity));
        }
        public static Type GetPrimaryKeyType(Type entityType)
        {
            foreach (var interfaceType in entityType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
                {
                    return interfaceType.GenericTypeArguments[0];
                }
            }

            throw new Exception("Can not find primary key type of given entity type: " + entityType + ". Be sure that this entity type implements IEntity<TPrimaryKey> interface");
        }

        public static void CopyPropertiesFrom<TPrimaryKey>(this IEntity<TPrimaryKey> instance, IEntity<TPrimaryKey> other)
        {
            PropertyInfo[] publicProperties = instance.GetType().GetProperties();
            PropertyInfo[] otherProperties = other.GetType().GetProperties();
            var typeOfThis = instance.GetType();
            var typeOfOther = other.GetType();
            if (!typeOfThis.IsAssignableFrom(typeOfOther) && !typeOfOther.IsAssignableFrom(typeOfThis))
            {
                throw new ArgumentException(string.Format("参数{0}必须为{1}类型", nameof(other), typeOfThis.FullName));
            }
            for (int i = 0; i < publicProperties.Length; i++)
            {
                object thisValue = publicProperties[i].GetValue(instance);
                object otherValue = otherProperties[i].GetValue(other);
                Type otherValueType = otherProperties[i].PropertyType;
                object defaultValue = otherValueType.IsValueType ? Activator.CreateInstance(otherValueType) : null;
                if (thisValue!=otherValue && otherValue!=defaultValue)
                {
                    
                    publicProperties[i].SetValue(instance, otherValue);
                }
            }
        }


    }
}
