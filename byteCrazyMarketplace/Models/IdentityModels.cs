using byteCrazy.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
namespace byteCrazy.Models
{

    public class ApplicationUser : IdentityUser
    {
        public string Hometown { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
       
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
       
            return userIdentity;
        }
    }

    // Database context
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserDetails> UserDetails { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public DbSet<AdminListModels> Listings { get; set; }
        public DbSet<ListingActivityLog> ListingActivityLogs { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<SavedProduct> SavedProducts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
         .ToTable("Product", "");

            modelBuilder.Entity<SavedProduct>()
                .HasKey(sp => new { sp.UserID, sp.ProductID });

            modelBuilder.Entity<SavedProduct>()
                .HasRequired(sp => sp.User)
                .WithMany()
                .HasForeignKey(sp => sp.UserID);

            modelBuilder.Entity<SavedProduct>()
                .HasKey(sp => new { sp.UserID, sp.ProductID });

            modelBuilder.Entity<SavedProduct>()
                .HasRequired(sp => sp.User)
                .WithMany()
                .HasForeignKey(sp => sp.UserID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SavedProduct>()
                .HasRequired(sp => sp.Product)
                .WithMany()
                .HasForeignKey(sp => sp.ProductID)
                .WillCascadeOnDelete(false);
        }
    }
}
