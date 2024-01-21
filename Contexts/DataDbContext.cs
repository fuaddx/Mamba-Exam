using Mamba.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Mamba.Contexts
{
    public class DataDbContext : IdentityDbContext
    {
        public DataDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }    

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry<Blog>> entries = ChangeTracker.Entries<Blog>();
            IEnumerable<EntityEntry<Setting>> entries2= ChangeTracker.Entries<Setting>();
            foreach (EntityEntry<Blog> entry in entries)
            {
                if(entry.State == EntityState.Added)
                {
                    DateTime dateTime = DateTime.UtcNow;
                    DateTime AzTime = dateTime.AddHours(4);
                    entry.Entity.CreatedTime = AzTime;
                    entry.Entity.UpdatedTime = null;
                }else if (entry.State == EntityState.Modified)
                {
                    DateTime dateTime = DateTime.UtcNow;
                    DateTime AzTime = dateTime.AddHours(4);
                    entry.Entity.UpdatedTime = AzTime;
                    var modifiedProps = entry.Properties.Where(prop => prop.IsModified && !prop.Metadata.IsPrimaryKey());
                    if(!modifiedProps.Any())
                    {
                        entry.Entity.UpdatedTime = null;

                    }
                }
            }
            foreach (EntityEntry<Setting> entry in entries2)
            {
                if (entry.State == EntityState.Modified)
                {
                    DateTime dateTime = DateTime.UtcNow;
                    DateTime AzTime = dateTime.AddHours(4);
                    entry.Entity.UpdatedTime = AzTime;
                    var modifiedProps = entry.Properties.Where(prop => prop.IsModified && !prop.Metadata.IsPrimaryKey());
                    if (!modifiedProps.Any())
                    {
                        entry.Entity.UpdatedTime = null;

                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>().HasData(new Setting
            {
                Id = 1,
                Title = "Mamba",
                Address = "Baki 44",
                Phone = "89985665",
                Email = "Baki@mail.ru"
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
