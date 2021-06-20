using Microsoft.EntityFrameworkCore;
using Raspi.Temperature.App.Server.Infrastructure.Configuration;
using Raspi.Temperature.App.Server.Infrastructure.Model;

namespace Raspi.Temperature.App.Server.Infrastructure
{
    public class RaspiDbContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }

        public RaspiDbContext(DbContextOptions<RaspiDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("SQ_tabTemperatureHistory_fId", schema: "dbo");
            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new TemperatureRecordConfiguration());
        }
    }
}
