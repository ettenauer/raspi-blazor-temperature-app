using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raspi.Temperature.App.Server.Infrastructure.Model;

namespace Raspi.Temperature.App.Server.Infrastructure.Configuration
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.ToTable("tabDevice");
            builder.Property(_ => _.Id)
                .HasColumnName("fId");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Name)
                .HasColumnName("fName");
            builder.Property(_ => _.Latitude)
    .           HasColumnName("fLatitude");
            builder.Property(_ => _.Longitude)
                .HasColumnName("fLongitude");
            builder.Property(_ => _.Active)
                .HasColumnName("fActive");
            builder.HasMany(_ => _.TemperatureHistory)
                .WithOne()
                .HasForeignKey("frDeviceId");
        }
    }
}
