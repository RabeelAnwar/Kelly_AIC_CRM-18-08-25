
using DataAccess.Audit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Service.Extensions;
using Service.Sessions;
//using Service.Sessions;


namespace Service.EntityAudit
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ISessionData _session;

        public AuditSaveChangesInterceptor(ISessionData sessionData)
        {
            _session = sessionData;
        }

        private void ApplyAudit(DbContext? context)
        {
            if (context == null) return;

            var userId = _session.UserId;

            var now = DateTime.UtcNow;

            var auditableEntities = context.ChangeTracker.Entries()
                .Where(e => e.Entity is IFullAuditedEntity)
                .ToList();

            foreach (var entry in auditableEntities)
            {
                var entity = (IFullAuditedEntity)entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatorUserId = userId;
                        entity.CreationTime = now;
                        entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entity.LastModifierUserId = userId;
                        entity.LastModificationTime = now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified; // State modified for soft delete
                        entity.IsDeleted = true;
                        entity.DeleterUserId = userId;
                        entity.DeletionTime = now;
                        break;

                }

                //if (entry.Entity is IMustHaveTenant mustHaveTenant &&
                //    (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted))
                //{
                //    mustHaveTenant.TenantId = _session.TenantId;
                //}

                if (entry.Entity is IMustHaveTenant mustHaveTenant &&
                    (entry.State == EntityState.Added && mustHaveTenant.TenantId == 0))
                {
                    mustHaveTenant.TenantId = _session.TenantId;
                }


                // TENANT LOGIC
                //if (entry.Entity is IMustHaveTenant mustHaveTenant)
                //{
                //    switch (entry.State)
                //    {
                //        case EntityState.Added:
                //            if (mustHaveTenant.TenantId == 0)
                //            {
                //                mustHaveTenant.TenantId = _session.TenantId;
                //            }
                //            break;

                //        case EntityState.Modified:
                //            var originalTenantId = (int)entry.OriginalValues[nameof(IMustHaveTenant.TenantId)];
                //            if (mustHaveTenant.TenantId != originalTenantId)
                //            {
                //                throw new InvalidOperationException("Changing the TenantId is not allowed. Please contact the admin!");
                //            }
                //            break;

                //        case EntityState.Deleted:
                //            goto case EntityState.Modified; // Fall-through to reuse same logic as soft-deleted entities

                //        default:
                //            if (entity.IsDeleted && mustHaveTenant.TenantId != _session.TenantId)
                //            {
                //                throw new UnauthorizedAccessException("Cannot delete an entity that doesn't belong to the current tenant. Please contact the admin!");
                //            }
                //            break;
                //    }
                //}


            }
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }



}