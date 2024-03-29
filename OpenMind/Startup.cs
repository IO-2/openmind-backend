using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using OpenMind.Installers;

namespace OpenMind
{
    public class Startup
    {
        //TODO: Make installers more flexible, that excepts creation of two same variables (check WebInstaller has same var)
        private readonly string MyAllowSpecificOrigins = "*";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IInstaller webInstaller = new WebInstaller();
            IInstaller dataInstaller = new DataInstaller();
            
            webInstaller.InstallServices(services, Configuration);
            dataInstaller.InstallServices(services, Configuration);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors(MyAllowSpecificOrigins);
            
	    app.UseHttpsRedirection();

	    app.UseStaticFiles();

	    app.UseCors(MyAllowSpecificOrigins);

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("app/privacy-policy", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "privacy.html")));
                });
                endpoints.MapGet("app/terms-of-use", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "terms-of-use.html")));
                });
            });
        }
    }
}
