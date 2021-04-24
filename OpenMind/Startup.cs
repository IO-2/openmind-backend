using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenMind.Data;
using OpenMind.Models;
using OpenMind.Options;
using OpenMind.Services;
using OpenMind.Services.Interfaces;
using OpenMind.Services.Validators;
using OpenMind.Services.Validators.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenMind
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "*";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtOptions = new JwtOptions();
            Configuration.Bind(nameof(JwtOptions), jwtOptions);

            services.AddSingleton(jwtOptions);

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IEmailValidator, EmailValidator>();
            services.AddScoped<IPasswordValidator, PasswordValidator>();
            services.AddScoped<ICourcesService, CourseService>();


            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };


            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = tokenValidationParameters;
                });

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("*")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddDbContext<DataContext>(x =>
            {
                x.UseLazyLoadingProxies()
                    .UseNpgsql(Configuration.GetConnectionString("Default"));
            });

            services.AddIdentity<UserModel, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                })
                .AddEntityFrameworkStores<DataContext>();

            services.AddControllers();
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("version");
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                var swaggerOptions = new SwaggerOptions();
                Configuration.GetSection("Swagger").Bind(swaggerOptions);
                
                app.UseSwagger(option => option.RouteTemplate = swaggerOptions.JsonRoute);
                app.UseSwaggerUI(option =>
                {
                    foreach (var currentVersion in swaggerOptions.Versions)
                    {
                        option.SwaggerEndpoint(currentVersion.UiEndpoint, $"{swaggerOptions.Title} {currentVersion.Name}");
                    }
                });
            }

            app.UseRouting();
            
            app.UseCors(MyAllowSpecificOrigins);
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "wwwroot")),
                RequestPath = "/wwwroot"
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}