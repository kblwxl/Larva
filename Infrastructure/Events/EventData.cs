using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    [Serializable]
    public abstract class EventData : IEventData
    {
        public object EventSource { get; set; }

        public DateTime EventTime { get; set; }
        public EventData()
        {
            EventTime = DateTime.Now;
        }
    }
}
