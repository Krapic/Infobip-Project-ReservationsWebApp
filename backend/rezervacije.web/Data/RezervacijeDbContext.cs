using Microsoft.EntityFrameworkCore;
using Rezervacije.Models;
using Rezervacije.Web.Models;

namespace Rezervacije.Data;

public class RezervacijeDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Business> Businesses { get; set; }
    public DbSet<BusinessActivity> BusinessActivities { get; set; }
    public DbSet<SixDigitAuthorization> SixDigitAuthorizations { get; set; }
    public DbSet<WorkingDayStructure> WorkingDaysStructure { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public RezervacijeDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //seeder podataka
        modelBuilder.Entity<Address>().HasData(
            new Address
            {
                Id = 1,
                Country = "Croatia",
                PostalCode = "44250",
                Town = "Petrinja",
                HouseNumber = "3B",
                Street = "Ul. Ive Maline"
            }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Email = "test@gmail.com",
                LastName = "test",
                Name = "test",
                Password = BCrypt.Net.BCrypt.HashPassword("test"),
                PhoneNumber = "993325806",
                IsAdmin = true,
                DialingCode = "385",
                AddressId = 1,
            }
        );

        modelBuilder.Entity<User>()
            .HasOne(x => x.Address)
            .WithOne()
            .HasForeignKey<User>(u => u.AddressId);

        modelBuilder.Entity<User>()
            .HasOne(x => x.Business)
            .WithOne(x => x.User)
            .HasForeignKey<User>(u => u.BusinessId);

        modelBuilder.Entity<Business>()
            .HasOne(x => x.Address)
            .WithOne()
            .HasForeignKey<Business>(b => b.AddressId);

        modelBuilder.Entity<Business>()
            .HasOne(x => x.User)
            .WithOne(x => x.Business)
            .HasForeignKey<Business>(x => x.UserId);

        modelBuilder.Entity<BusinessActivity>()
            .HasOne(x => x.Business)
            .WithMany(x => x.BusinessActivities)
            .HasForeignKey(x => x.BusinessId);

        modelBuilder.Entity<WorkingDayStructure>()
            .HasOne(x => x.Business)
            .WithMany(x => x.WorkingDayStructures)
            .HasForeignKey(x => x.BusinessId);

        modelBuilder.Entity<Appointment>()
            .HasOne(x => x.Business)
            .WithMany(x => x.BusinessAppointments)
            .HasForeignKey(x => x.BusinessId);

        modelBuilder.Entity<Appointment>()
            .HasOne(x => x.ReservedUser)
            .WithMany(x => x.Appointments)
            .HasForeignKey(x => x.ReservedUserId);

        base.OnModelCreating(modelBuilder);
    }
}
