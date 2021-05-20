using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenMind.Data;
using OpenMind.Models.Users;
using OpenMind.Services;
using OpenMind.Services.Interfaces;
using OpenMind.Services.Validators;
using OpenMind.Services.Validators.Interfaces;

namespace OpenMind.Installers
{
    public class DataInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<DataContext>(x =>
            {
                x.UseLazyLoadingProxies()
                    .UseNpgsql(configuration.GetConnectionString("Default"));
            }); 
            
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IMediaService, MediaService>();
            services.AddTransient<IEmailValidator, EmailValidator>();
            services.AddTransient<IPasswordValidator, PasswordValidator>();
            services.AddTransient<ICoursesService, CourseService>();

            services.AddIdentity<UserModel, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                })
                .AddEntityFrameworkStores<DataContext>();

            services.AddControllers();
        }
    }
}
