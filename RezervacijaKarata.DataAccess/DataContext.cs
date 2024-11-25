using Microsoft.EntityFrameworkCore;
using RezervacijaKarata.Domain.Model;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingDayZone> BookingDayZones { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<RaceDay> RaceDays { get; set; }
    public DbSet<SeatingZone> SeatingZones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Define primary keys
        modelBuilder.Entity<PromoCode>().HasKey(pc => pc.Id);
        modelBuilder.Entity<Customer>().HasKey(c => c.Id);
        modelBuilder.Entity<Booking>().HasKey(b => b.Id);
        modelBuilder.Entity<RaceDay>().HasKey(rd => rd.Id);
        modelBuilder.Entity<SeatingZone>().HasKey(sz => sz.Id);
        modelBuilder.Entity<BookingDayZone>().HasKey(bdz => new { bdz.BookingId, bdz.RaceDayId, bdz.SeatingZoneId });

        // Define relationships
        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Customer)
            .WithOne(c => c.Booking) // Ensure the Customer class has a Booking navigation property
            .HasForeignKey<Booking>(b => b.CustomerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookingDayZone>()
            .HasOne(bdz => bdz.Booking)
            .WithMany(b => b.BookingDayZones)
            .HasForeignKey(bdz => bdz.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookingDayZone>()
            .HasOne(bdz => bdz.RaceDay)
            .WithMany()
            .HasForeignKey(bdz => bdz.RaceDayId);

        modelBuilder.Entity<BookingDayZone>()
            .HasOne(bdz => bdz.SeatingZone)
            .WithMany()
            .HasForeignKey(bdz => bdz.SeatingZoneId);

        modelBuilder.Entity<PromoCode>()
        .HasOne<Booking>() // Definisanje relacije bez navigacionog svojstva u Booking
        .WithMany()         // Booking nema kolekciju PromoCodes
        .HasForeignKey(pc => pc.BookingId) // Foreign key koji se veže na BookingId
        .IsRequired()                      // BookingId je obavezno polje
        .OnDelete(DeleteBehavior.Cascade); // Ako se rezervacija obriše, obriši i povezane promo kodove
    }
}
