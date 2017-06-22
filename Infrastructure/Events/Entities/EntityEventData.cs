using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events.Entities
{
    [Serializable]
    public class EntityEventData<TEntity> : EventData, IEventDataWithInheritableGenericArgument
    {
        public TEntity Entity { get; private set; }
        public EntityEventData(TEntity entity)
        {
            Entity = entity;
        }
        public object[] GetConstructorArgs()
        {
            return new object[] { Entity };
        }
    }
}
