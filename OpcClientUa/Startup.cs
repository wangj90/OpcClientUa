using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpcClientUa.Middleware;
using OpcClientUa.SignalR;
using System.Collections.Generic;

namespace OpcClientUa
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseSignalR(routes => { routes.MapHub<OpcHub>("/opcHub"); });

            app.UseOpcNotification(new OpcPointOptions
            {
                OpcPoints = new List<string>
                {
                    "Data Type Examples.16 Bit Device.R Registers.Boolean2",
                    "Data Type Examples.16 Bit Device.R Registers.Double2",
                    "Data Type Examples.16 Bit Device.R Registers.DWord2",
                    "Simulation Examples.Functions.Ramp1"
                }
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
