using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DriverApp.Models;
using Microsoft.EntityFrameworkCore;
using DriverApp.Services;
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
            services.AddCors();
            services.AddMvc();

            var connection = @"Server=(localdb)\mssqllocaldb;Database=cloudrouting_db2;Trusted_Connection=True;";
            services.AddDbContext<ApiContext>(options => options.UseSqlServer(connection));

            services.AddTransient<DbRepository>();
			services.AddSingleton<CloudApi>();

			services.AddAuthorization(options =>
            {
                options.AddPolicy("ManagersOnly", policy => policy.RequireClaim("Role", "Manager"));
                options.AddPolicy("DriversOnly", policy => policy.RequireClaim("Role", "Driver"));
				options.AddPolicy("AllUsers", policy => policy.RequireAssertion(context =>
					context.User.HasClaim(c =>
						c.Type == "Role" && (c.Value == "Manager" || c.Value == "Driver"))
					)
				);
			});

            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ApiContext ctx)
        {
            
            loggerFactory.AddDebug();

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "CookieAuth",
                AutomaticAuthenticate = true
            });

            Mapper.Initialize(config =>
            {
                config.CreateMap<Driver, SendDriverDto>();
            });

            app.UseMvc();


            app.UseStaticFiles();

			AddTestData(ctx);
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
                DriverId = "1",
				CustomerKey = "test"
			};
			var driver2 = new Driver
			{
				Manager = user,
				DriverId = "2",
				CustomerKey = "test"
			};
			var driver3 = new Driver
			{
				Manager = user,
				DriverId = "3",
				CustomerKey = "test"
			};

			user.Drivers.Add(driver);
			user.Drivers.Add(driver2);
			user.Drivers.Add(driver3);

			context.Managers.Add(user);
            context.Drivers.Add(driver);
			context.Drivers.Add(driver2);
			context.Drivers.Add(driver3);

			context.SaveChanges();
        }
    }
}
