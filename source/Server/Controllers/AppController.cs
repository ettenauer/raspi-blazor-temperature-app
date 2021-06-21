using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Raspi.Temperature.App.Server.Extensions;
using Raspi.Temperature.App.Server.Infrastructure;
using Raspi.Temperature.App.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Raspi.Temperature.App.Server.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "AppUser")]
    public class AppController : ControllerBase
    {
        private readonly IDbContextFactory<RaspiDbContext> dbContextFactory;

        public AppController(IDbContextFactory<RaspiDbContext> dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        [HttpGet]
        [Route("Devices")]
        public async Task<IEnumerable<Device>> GetDevicesAsync()
        {
            using var context = dbContextFactory.CreateDbContext();
            var activeDevices = await context.Devices
                .Where(_ => _.Active)
                .ToListAsync()
                .ConfigureAwait(false);

            return activeDevices.MapDevices();
        }

        [HttpGet]
        [Route("DeviceDashboard")]
        public async Task<IActionResult> GetDeviceDashboardAsync(int deviceId)
        {
            using var context = dbContextFactory.CreateDbContext();
            var device = await context.Devices
                .Include(_ => _.TemperatureHistory)
                .Where(_ => _.Id == deviceId)
                .SingleOrDefaultAsync()
                .ConfigureAwait(false);

            if (device == null)
                return BadRequest();

            return Ok(device.MapDeviceDashboard());
        }
    }
}
