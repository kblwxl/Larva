﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public interface IDictionaryBasedConfig
    {
        void Set<T>(string name, T value);
        object Get(string name);
        T Get<T>(string name);
        object Get(string name, object defaultValue);
        T Get<T>(string name, T defaultValue);
        T GetOrCreate<T>(string name, Func<T> creator);
    }
}
