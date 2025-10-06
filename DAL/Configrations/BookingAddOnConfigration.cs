using DAL.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configrations
{
    internal class BookingAddOnConfigration : IEntityTypeConfiguration<BookingAddOn>
    {
        public void Configure(EntityTypeBuilder<BookingAddOn> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Booking)
            .WithMany(s => s.BookingAddOns)
            .HasForeignKey(e => e.BookingID)
            .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(e => e.AddOn)
            .WithMany(s => s.BookingAddOns)
            .HasForeignKey(e => e.AddOnID)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.BookingID, e.AddOnID })
           .IsUnique();

        builder.Property(e => e.Quantity)
              .IsRequired()
              .HasColumnType("smallint");

            builder.Property(e => e.TotalPrice)
             .IsRequired();
        }
    }
}
