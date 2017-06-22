using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events.Entities
{
    public class NullEntityChangeEventHelper : IEntityChangeEventHelper
    {
        
        public static NullEntityChangeEventHelper Instance { get { return SingletonInstance; } }
        private static readonly NullEntityChangeEventHelper SingletonInstance = new NullEntityChangeEventHelper();

        private NullEntityChangeEventHelper()
        {

        }

        public void TriggerEntityCreatingEvent(object entity)
        {

        }

        public void TriggerEntityCreatedEventOnUowCompleted(object entity)
        {

        }

        public void TriggerEntityUpdatingEvent(object entity)
        {

        }

        public void TriggerEntityUpdatedEventOnUowCompleted(object entity)
        {

        }

        public void TriggerEntityDeletingEvent(object entity)
        {

        }

        public void TriggerEntityDeletedEventOnUowCompleted(object entity)
        {

        }
    }
}
