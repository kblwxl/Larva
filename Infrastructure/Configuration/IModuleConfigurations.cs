using System;

namespace Infrastructure.Configuration
{
    public interface IModuleConfigurations
    {
        IStartupConfiguration StartupConfiguration { get; }
    }
}