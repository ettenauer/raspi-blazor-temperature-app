using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using Raspi.Temperature.App.Server.Extensions;
using Raspi.Temperature.App.Server.Infrastructure;
using Raspi.Temperature.App.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Raspi.Temperature.App.Server.Controllers
{
    //[RequiredScope("API.Access")]
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> logger;
        private readonly IDbContextFactory<RaspiDbContext> dbContextFactory;

        public DashboardController(IDbContextFactory<RaspiDbContext> dbContextFactory, ILogger<DashboardController> logger)
        {
            this.dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
