
using System.Linq.Expressions;
using DataAccess.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataAccess.Entities
{

    public class MainContext : IdentityDbContext<User, IdentityRole, string>
    {

        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<CallType> CallTypes { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }
        public DbSet<SkillMaster> SkillMasters { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<ClientManager> ClientManagers { get; set; }
        public DbSet<ClientPipeline> ClientPipelines { get; set; }
        public DbSet<ConsultantInterview> ConsultantInterviews { get; set; }
        public DbSet<ConsultantReference> ConsultantReferences { get; set; }
        public DbSet<CallRecord> CallRecords { get; set; }
        public DbSet<DocumentUpload> DocumentUploads { get; set; }
        public DbSet<ClientRequisition> ClientRequisitions { get; set; }
        public DbSet<ConsultantActivity> ConsultantActivities { get; set; }
        public DbSet<ConsultantInterviewProcess> ConsultantInterviewProcesses { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<UsersLog> UsersLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<User>();
            //builder.Entity<Role>();

            builder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.ContactType)
                      .WithMany()
                      .HasForeignKey(u => u.ContactTypeId);

                entity.HasIndex(u => u.PersonalEmail).IsUnique();
                entity.HasIndex(u => u.TenantId);

            });

            // Configure IdentityRole if needed
            builder.Entity<IdentityRole>(entity =>
            {
                entity.HasIndex(r => r.Name).IsUnique();
            });


            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (!typeof(IFullAuditedEntity).IsAssignableFrom(clrType))
                    continue;

                var rootType = entityType.GetRootType()?.ClrType ?? clrType;

                if (rootType != clrType)
                    continue;

                var parameter = Expression.Parameter(clrType, "e");
                var isDeletedProperty = Expression.Property(parameter, nameof(IFullAuditedEntity.IsDeleted));
                var isDeletedCondition = Expression.Equal(isDeletedProperty, Expression.Constant(false));
                var lambda = Expression.Lambda(isDeletedCondition, parameter);

                builder.Entity(clrType).HasQueryFilter(lambda);
            }
        }
    }
}