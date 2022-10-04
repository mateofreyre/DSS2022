using DSS2022.Data.Configurations;
using DSS2022.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.Data
{
    public class DSS2022DataContext : DbContext
    {
        public DSS2022DataContext(DbContextOptions<DSS2022DataContext> options) : base(options)
        {
            //_contextService = contextService;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Collection> Collections { get; set; }


        /*public virtual DbSet<UserRole> UsersRoles { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolesPermissions { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CollectionConfiguration());

            //modelBuilder.Seed();

        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

    }
}
