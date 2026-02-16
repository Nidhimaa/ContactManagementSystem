using ContactManagementSystem.Repositories.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactManagementSystem.Repositories
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ContactManagement> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ContactManagement>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                      .IsRequired()
                      .HasMaxLength(15);

                entity.Property(x => x.Email)
                      .IsRequired()
                      .HasMaxLength(30);

                entity.Property(x => x.MobileNo)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(x => x.Address)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(x => x.IsDeleted)
                      .HasDefaultValue(false);
            });
        }
    }
}