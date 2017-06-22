using Infrastructure.Dependency;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.EF
{
    public sealed class SimpleDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        

        public SimpleDbContextProvider()
        {
            //DbContext = dbContext;
        }
        public TDbContext GetDbContext()
        {
            string connectionString=IocManager.Instance.Resolve<IConnectionStringResolver>().GetNameOrConnectionString();
            var retValue = IocManager.Instance.Resolve<IDbContextResolver>().Resolve<TDbContext>(connectionString);
            //SimpleDbContextHolder.AddContext(retValue);
            return retValue;
        }
        
    }

    //static class SimpleDbContextHolder
    //{
    //    private static Dictionary<int, List<IDisposable>> activitedContext = new Dictionary<int, List<IDisposable>>();

    //    public static void AddContext(IDisposable value)
    //    {
    //        var threadCode = System.Threading.Thread.CurrentThread.GetHashCode();
    //        Console.WriteLine($"AddContext,thread={threadCode}");
    //        List<IDisposable> list;
    //        if (activitedContext.ContainsKey(threadCode))
    //        {
    //            list = activitedContext[threadCode];
    //        }
    //        else
    //        {
    //            list = new List<IDisposable>();
    //            activitedContext.Add(threadCode, list);
    //        }
    //        list.Add(value);
    //    }
    //    public static void DisposeContext()
    //    {
    //        var key = System.Threading.Thread.CurrentThread.GetHashCode();
    //        Console.WriteLine($"DisposeContext,thread={key}");
    //        if (activitedContext.ContainsKey(key))
    //        {
    //            activitedContext[key].ForEach(x => x.Dispose());
    //            activitedContext.Remove(key);
    //        }
    //    }
    //}
}
