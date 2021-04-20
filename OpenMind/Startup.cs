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
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OpenMind
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
            var jwtOptions = new JwtOptions();
            Configuration.Bind(nameof(JwtOptions), jwtOptions);
            
            services.AddSingleton(jwtOptions);
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IChecklistService, ChecklistService>();
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });
            
            services.AddDbContext<DataContext>(x =>
            {
                x.UseLazyLoadingProxies()
                    .UseNpgsql(Configuration.GetConnectionString("Default"));
            });
            
            services.AddIdentity<UserModel, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddControllers();
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("version");
            });
            
            services.AddSwaggerGen(c =>
            {
                var swaggerOptions = new SwaggerOptions();
                Configuration.GetSection("Swagger").Bind(swaggerOptions);
            
                foreach (var currentVersion in swaggerOptions.Versions)
                {
                    c.SwaggerDoc(currentVersion.Name, new OpenApiInfo
                    {
                        Title = swaggerOptions.Title,
                        Version = currentVersion.Name,
                        Description = swaggerOptions.Description
                    });
                }

                c.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo))
                    {
                        return false;
                    }
                    var versions = methodInfo.DeclaringType.GetConstructors()
                        .SelectMany(constructorInfo => constructorInfo.DeclaringType.CustomAttributes
                            .Where(attributeData => attributeData.AttributeType == typeof(ApiVersionAttribute))
                            .SelectMany(attributeData => attributeData.ConstructorArguments
                                .Select(attributeTypedArgument => attributeTypedArgument.Value)));

                    return versions.Any(v => $"{v}" == version);
                });
                
                c.EnableAnnotations();
                
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[0]}
                };
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                
                // Dictionary
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
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