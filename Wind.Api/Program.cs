using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DarkSky.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wind.Api.Models;
using Wind.Api.Services;
using Wind.Api.Workers;

namespace Wind.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .RunWorkers()
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }

    public static class IWebHostExtenstions
    {
        public static IWebHost RunWorkers(this IWebHost webHost)
        {
            var serviceScopeFactory = (IServiceScopeFactory)webHost.Services.GetService(typeof(IServiceScopeFactory));
            var scope = serviceScopeFactory.CreateScope();
            var services = scope.ServiceProvider;

            var dbContext = services.GetRequiredService<WindDbContext>();
            var timeMashineService = services.GetRequiredService<ITimeMashineService>();
            var apiCallsTrackerService = services.GetRequiredService<IApiCallsTrackerService>();
            var pointService = services.GetRequiredService<IPointService>();
            var windDayService = services.GetRequiredService<IWindDayService>();
            var windHourService = services.GetRequiredService<IWindHourService>();

            var darkSkyWorker = new DarkApiWorker(
                timeMashineService,
                apiCallsTrackerService,
                pointService,
                windDayService,
                windHourService
            );

            darkSkyWorker.Start();

            return webHost;
        }
    }
}
