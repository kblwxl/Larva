using EntityFramework.DynamicFilters;
using Infrastructure;
using Infrastructure.Domain;
using Infrastructure.Events.Entities;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Seedwork.EF
{
    public abstract class BaseDbContext : DbContext,Infrastructure.Dependency.ITransientDependency,Infrastructure.IShouldInitialize
    {
        public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }
        public ILogger Logger { get; set; }
        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }
        protected BaseDbContext()
        {
            InitializeDbContext();
        }
        protected BaseDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            InitializeDbContext();
        }
        protected BaseDbContext(DbCompiledModel model)
            : base(model)
        {
            InitializeDbContext();
        }
        protected BaseDbContext(DbConnection existingConnection, bool contextOwnsConnection)
            : base(existingConnection, contextOwnsConnection)
        {
            InitializeDbContext();
        }
        protected BaseDbContext(string nameOrConnectionString, DbCompiledModel model)
            : base(nameOrConnectionString, model)
        {
            InitializeDbContext();
        }
        protected BaseDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext)
            : base(objectContext, dbContextOwnsObjectContext)
        {
            InitializeDbContext();
        }
        protected BaseDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
            : base(existingConnection, model, contextOwnsConnection)
        {
            InitializeDbContext();
        }
        private void InitializeDbContext()
        {
            SetNullsForInjectedProperties();
            RegisterToChanges();
        }
        private void RegisterToChanges()
        {
            ((IObjectContextAdapter)this)
                .ObjectContext
                .ObjectStateManager
                .ObjectStateManagerChanged += ObjectStateManager_ObjectStateManagerChanged;
        }
        protected virtual void ObjectStateManager_ObjectStateManagerChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            var contextAdapter = (IObjectContextAdapter)this;
            if (e.Action != CollectionChangeAction.Add)
            {
                return;
            }

            var entry = contextAdapter.ObjectContext.ObjectStateManager.GetObjectStateEntry(e.Element);
            switch (entry.State)
            {
                case EntityState.Added:
                    CheckAndSetId(entry.Entity);
                    
                    SetCreationAuditProperties(entry.Entity);
                    break;
                    
            }
        }
        private void SetNullsForInjectedProperties()
        {
            Logger = NullLogger.Instance;
            
            EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
            
        }
        public virtual void Initialize()
        {
            Database.Initialize(false);
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Filter(DataFilters.SoftDelete, (ISoftDelete d) => d.IsDeleted, false);
            
        }
        public override int SaveChanges()
        {
            try
            {
                ApplyAbpConcepts();
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                LogDbEntityValidationException(ex);
                throw;
            }
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                ApplyAbpConcepts();
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException ex)
            {
                LogDbEntityValidationException(ex);
                throw;
            }
        }
        protected virtual void ApplyAbpConcepts()
        {
            

            var entries = ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        EntityChangeEventHelper.TriggerEntityCreatingEvent(entry.Entity);
                        EntityChangeEventHelper.TriggerEntityCreatedEventOnUowCompleted(entry.Entity);
                        break;
                    case EntityState.Modified:
                        SetModificationAuditProperties(entry);
                        if (entry.Entity is ISoftDelete && (entry.Entity as ISoftDelete).IsDeleted)
                        {
                            SetDeletionAuditProperties(entry.Entity);
                            EntityChangeEventHelper.TriggerEntityDeletingEvent(entry.Entity);
                            EntityChangeEventHelper.TriggerEntityDeletedEventOnUowCompleted(entry.Entity);
                        }
                        else
                        {
                            EntityChangeEventHelper.TriggerEntityUpdatingEvent(entry.Entity);
                            EntityChangeEventHelper.TriggerEntityUpdatedEventOnUowCompleted(entry.Entity);
                        }

                        break;
                    case EntityState.Deleted:
                        CancelDeletionForSoftDelete(entry);
                        SetDeletionAuditProperties(entry.Entity);
                        EntityChangeEventHelper.TriggerEntityDeletingEvent(entry.Entity);
                        EntityChangeEventHelper.TriggerEntityDeletedEventOnUowCompleted(entry.Entity);
                        break;
                }
            }
        }
        protected virtual void CheckAndSetId(object entityAsObj)
        {
            //Set GUID Ids
            var entity = entityAsObj as IEntity<Guid>;
            if (entity != null && entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }
        }
        protected virtual void SetCreationAuditProperties(object entityAsObj)
        {
            if (entityAsObj is IHasCreationTime)
            {
                ((IHasCreationTime)entityAsObj).CreationTime = DateTime.Now;
            }
            
        }
        protected virtual void SetModificationAuditProperties(DbEntityEntry entry)
        {
            if (entry.Entity is IHasUpdationTime)
            {
                entry.Cast<IHasUpdationTime>().Entity.UpdationTime = DateTime.Now;
            }
        }
        protected virtual void CancelDeletionForSoftDelete(DbEntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            var softDeleteEntry = entry.Cast<ISoftDelete>();

            softDeleteEntry.State = EntityState.Unchanged; //TODO: Or Modified? IsDeleted = true makes it modified?
            softDeleteEntry.Entity.IsDeleted = true;
        }
        protected virtual void SetDeletionAuditProperties(object entityAsObj)
        {
            if (entityAsObj is IHasDeletionTime)
            {
                var entity = (IHasDeletionTime)entityAsObj;

                if (entity.DeletionTime == null)
                {
                    entity.DeletionTime = DateTime.Now;
                }
            }
            
        }
        protected virtual void LogDbEntityValidationException(DbEntityValidationException exception)
        {
            Logger.Error("EntityFramework 校验错误:");
            foreach (var ve in exception.EntityValidationErrors.SelectMany(eve => eve.ValidationErrors))
            {
                Logger.Error(" - " + ve.PropertyName + ": " + ve.ErrorMessage);
            }
        }
    }
}
