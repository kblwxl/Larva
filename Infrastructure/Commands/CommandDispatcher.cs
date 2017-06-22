using Infrastructure.Commands.Factories;
using Infrastructure.Threading.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    internal class CommandDispatcher
    {
        private ILogger logger;
        private readonly ConcurrentDictionary<Type, List<ICommandHandlerFactory>> handlers = new ConcurrentDictionary<Type, List<ICommandHandlerFactory>>();

        public CommandDispatcher(Dependency.IIocResolver resolver)
        {
            logger = resolver.Resolve<ILogger>();
        }
        private IList<ICommandHandlerFactory> GetOrCreateHandlerFactories(Type commandType)
        {
            return handlers.GetOrAdd(commandType, (type) => new List<ICommandHandlerFactory>());
        }
        private IEnumerable<ICommandHandlerFactory> GetHandlerFactories(Type commandType)
        {
            var handlerFactoryList = new List<ICommandHandlerFactory>();

            foreach (var handlerFactory in handlers.Where(hf => ShouldTriggerCommandForHandler(commandType, hf.Key)))
            {
                handlerFactoryList.AddRange(handlerFactory.Value);
            }

            return handlerFactoryList.ToArray();
        }

        private bool ShouldTriggerCommandForHandler(Type commandType, Type handlerType)
        {
            
            if (handlerType == commandType)
            {
                return true;
            }
            if (handlerType.IsAssignableFrom(commandType))
            {
                return true;
            }

            return false;
        }

        public void RegisteType(Type commandType,ICommandHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(commandType).Locking(factories => factories.Add(factory));

        }
        public async void DispatchCommand(ICommand command) 
        {
            //ExecutionContext.SuppressFlow();
            await Task.Run(()=>
            {
                try
                {
                    IntenalDispatch(command);
                }
                catch(Exception error)
                {
                    logger.Error(error.Message, error);
                }
                
            });
            //ExecutionContext.RestoreFlow();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void IntenalDispatch(ICommand command)
        {
            var commandType = command.GetType();
            foreach (var factory in GetHandlerFactories(commandType))
            {
                var handler = factory.GetHandler();
                if(handler==null)
                {
                    throw new Exception($"为 {commandType.Name} 注册的事件处理程序未实行 ICommandHandler<{commandType.Name}> 接口!");
                }
                var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
                try
                {
                    handlerType.
                        GetMethod("Handler", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, new[] { commandType }, null)
                        .Invoke(handler, new object[] { command });
                }
                finally
                {
                    factory.ReleaseHandler(handler);
                }

            }
        }
        
    }
}
