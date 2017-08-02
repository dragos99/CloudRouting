using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DriverApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using DriverApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using AutoMapper;
using DriverApp.Dtos;

namespace DriverApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connection = @"Server=tcp:cloudroutingort1dbserver.database.windows.net,1433;Initial Catalog=CLOUDROUTINGORT1_db;Persist Security Info=False;User ID=crort_user;Password=Ortec1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            services.AddDbContext<ApiContext>(options => options.UseSqlServer(connection));

            services.AddTransient<DbRepository>();
			services.AddSingleton<CloudApi>();

			services.AddAuthorization(options =>
            {
                options.AddPolicy("ManagersOnly", policy => policy.RequireClaim("Role", "Manager"));
                options.AddPolicy("DriversOnly", policy => policy.RequireClaim("Role", "Driver"));
            });

            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CloudApi cloudApi)
        {

            loggerFactory.AddDebug();

            var context = app.ApplicationServices.GetService<ApiContext>();
			cloudApi.GetRouteAsync();

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "CookieAuth",
                AutomaticAuthenticate = true
            });

            Mapper.Initialize(config =>
            {
                config.CreateMap<Driver, SendDriverDto>();
				config.CreateMap<ReceiveOrderDto, Order>();
            });

            app.UseMvc();


            app.UseStaticFiles();
        }

        private void AddTestData(ApiContext context)
        {
            if (context.Managers.Any()) return;

            var user = new Manager
            {
                CustomerKey = "test",
                Drivers = new List<Driver>()
            };

            var driver = new Driver
            {
                Manager = user,
                DriverId = "1"
            };

            user.Drivers.Add(driver);
            
            context.Managers.Add(user);
            context.Drivers.Add(driver);

            context.SaveChanges();
        }
    }
}
