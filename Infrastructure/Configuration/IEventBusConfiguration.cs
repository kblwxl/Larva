using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public interface IEventBusConfiguration
    {
        bool UseDefaultEventBus { get; set; }
    }
}
