using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;
using DarkSky.Api.Models;
using DarkSky.Api;
using Wind.Api.Models;
using Microsoft.EntityFrameworkCore;
using Wind.Api.Services;
using Wind.Api.Workers;
using Microsoft.AspNetCore.Cors;

namespace Wind.Api
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors();

      services.Configure<DarkSkySettings>(Configuration.GetSection("DarkSky"));
      services.AddScoped<ITimeMashineService, TimeMashineService>();

      services.AddScoped<IApiCallsTrackerService, ApiCallsTrackerService>();
      services.AddScoped<IPointService, PointService>();
      services.AddScoped<IWindDayService, WindDayService>();
      services.AddScoped<IWindHourService, WindHourService>();

      services.AddDbContext<WindDbContext>(options =>
          options.UseMySql(Configuration.GetConnectionString("DefaultConnection"))
      );

      services.AddMvc();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info
        {
          Title = "Wind API",
          Version = "v1",
          Contact = new Contact
          {
            Name = "Ihor Zhvanko",
            Email = "igzhva@gmail.com",
            Url = "https://ihor-zhvanko.github.io/homepage"
          },
          Description = "Simple project based on .NET Core Web API." +
                  "Server provides wind data."
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseCors(builder =>
        builder.AllowAnyOrigin().AllowAnyHeader()
      );

      app.UseSwagger();

      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wind API V1");
        c.RoutePrefix = string.Empty;

      });

      app.UseMvc();
    }
  }
}
