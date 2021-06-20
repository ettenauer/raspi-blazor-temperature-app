using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raspi.Temperature.App.Server.Infrastructure.Model;
using System;

namespace Raspi.Temperature.App.Server.Infrastructure.Configuration
{
    public class TemperatureRecordConfiguration : IEntityTypeConfiguration<TemperatureRecord>
    {
        public void Configure(EntityTypeBuilder<TemperatureRecord> builder)
        {
            builder.ToTable("tabTemperatureHistory");
            builder.Property(_ => _.Id)
                .HasColumnName("fId")
                .HasDefaultValueSql("NEXT VALUE FOR dbo.SQ_tabTemperatureHistory_fId");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Date)
                .HasColumnName("fDate");
            builder.Property(_ => _.DegreeCelsius)
                .HasColumnName("fDegreeCelsius");
        }
    }
}
