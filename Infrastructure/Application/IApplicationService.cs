﻿using Infrastructure.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application
{
    public interface IApplicationService : ITransientDependency
    {
    }
}
