using Data.Seedwork.EF.Repositories;
using Infrastructure.Collections.Extensions;
using Infrastructure.Dependency;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seedwork.EF
{
    public class DbContextTypeMatcher : IDbContextTypeMatcher, ISingletonDependency
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
        private readonly Dictionary<Type, List<Type>> _dbContextTypes;
        public DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
            _dbContextTypes = new Dictionary<Type, List<Type>>();
        }
        public virtual void Add(Type sourceDbContextType, Type targetDbContextType)
        {
            if (!_dbContextTypes.ContainsKey(sourceDbContextType))
            {
                _dbContextTypes[sourceDbContextType] = new List<Type>();
            }

            _dbContextTypes[sourceDbContextType].Add(targetDbContextType);
        }

        public Type GetConcreteType(Type sourceDbContextType)
        {
            var allTargetTypes = _dbContextTypes.GetOrDefault(sourceDbContextType);

            if (allTargetTypes.IsNullOrEmpty())
            {
                //Not found any target type, return the given type if it's not abstract
                if (sourceDbContextType.IsAbstract)
                {
                    throw new Exception("Could not find a concrete implementation of given DbContext type: " + sourceDbContextType.AssemblyQualifiedName);
                }

                return sourceDbContextType;
            }

            if (allTargetTypes.Count == 1)
            {
                //Only one type does exists, return it
                return allTargetTypes[0];
            }

            //Will decide the target type with current UOW, so it should be in a UOW.
            if (_currentUnitOfWorkProvider.Current == null)
            {
                throw new Exception("GetConcreteType method should be called in a UOW.");
            }

            

            //Try to get the DbContext which not defined AutoRepositoryTypesAttribute
            var defaultRepositoryContexes = allTargetTypes
                .Where(type => !type.IsDefined(typeof(AutoRepositoryTypesAttribute), true))
                .ToList();

            if (defaultRepositoryContexes.Count == 1)
            {
                return defaultRepositoryContexes[0];
            }

            throw new Exception(string.Format(
                "Found more than one concrete type for given DbContext Type ({0}).",
                sourceDbContextType
                
                ));
        }

        public void Populate(Type[] dbContextTypes)
        {
            foreach (var dbContextType in dbContextTypes)
            {
                var types = new List<Type>();
                AddWithBaseTypes(dbContextType, types);
                foreach (var type in types)
                {
                    Add(type, dbContextType);
                }
            }
        }
        private static void AddWithBaseTypes(Type dbContextType, List<Type> types)
        {
            types.Add(dbContextType);
            if (dbContextType != typeof(BaseDbContext))
            {
                AddWithBaseTypes(dbContextType.BaseType, types);
            }
        }
    }
}
