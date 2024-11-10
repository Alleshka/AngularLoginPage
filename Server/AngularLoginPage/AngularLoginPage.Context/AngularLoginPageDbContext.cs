using AngularLoginPage.Domain;
using Microsoft.EntityFrameworkCore;

namespace AngularLoginPage.Context
{
    public class AngularLoginPageDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }

        public AngularLoginPageDbContext(DbContextOptions<AngularLoginPageDbContext> options) : base(options)
        {
            // Database?.EnsureDeleted();
            Database?.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(x => x.Id);
                u.HasIndex(x => x.Login);
                u.Property(x => x.Login).IsRequired();

                u.HasOne(x => x.Province)
                .WithMany()
                .HasForeignKey(x => x.ProvinceId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<User>().ToTable(t => t.HasCheckConstraint("CK_Username_MinLength", "LENGTH(Login) > 1"));

            modelBuilder.Entity<Country>(c =>
            {
                c.HasKey(x => x.Id);
                c.HasIndex(x => x.Name);
            });

            modelBuilder.Entity<Province>(p =>
            {
                p.HasKey(x => x.Id);
                p.HasIndex(x => x.Name);

                p.HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            FillData(modelBuilder);
        }

        protected void FillData(ModelBuilder modelBuilder)
        {
            Country georgia = new Country()
            {
                Id = Guid.Parse("603b2390-51f5-49a4-9d96-b18dda3364e1"),
                Name = "Georgia"
            };

            Country germany = new Country()
            {
                Id = Guid.Parse("f26bbe6c-e143-44ae-a706-5a218f6aff48"),
                Name = "Germany"
            };

            modelBuilder.Entity<Country>().HasData(georgia, germany);

            Province tbilisi = new Province()
            {
                Id = Guid.NewGuid(),
                CountryId = georgia.Id,
                Name = "Tbilisi"
            };

            Province batumi = new Province()
            {
                Id = Guid.NewGuid(),
                CountryId = georgia.Id,
                Name = "Batumi"
            };

            Province berlin = new Province()
            {
                Id = Guid.Parse("b5b16571-66d6-4d75-86de-cf8272392440"),
                CountryId = germany.Id,
                Name = "Berlin"
            };

            Province drezden = new Province()
            {
                Id = Guid.NewGuid(),
                CountryId = germany.Id,
                Name = "Drezden"
            };

            modelBuilder.Entity<Province>().HasData(tbilisi, batumi, berlin, drezden);
        }
    }
}
