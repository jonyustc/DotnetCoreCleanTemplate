using Clean.Application.Bases;
using Clean.Application.Interfaces;
using Clean.Persistence.Repositories;
using Clean.Persistence.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clean.Domain.Entities;

namespace Clean.Persistence
{
    public static class PersistenceDependencies
    {
        public static IServiceCollection AddPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAuthService, AuthService>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddDbContext<ApplicationDbContext>(option =>
  option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

            //

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            // .AddJwtBearer(o =>
            // {
            //     o.RequireHttpsMetadata = false;
            //     o.SaveToken = true;
            //     o.TokenValidationParameters = new TokenValidationParameters
            //     {
            //         ValidateIssuerSigningKey = true,
            //         ValidateIssuer = true,
            //         ValidateAudience = true,
            //         ValidateLifetime = true,
            //         ValidIssuer = configuration["JwtSettings:Issuer"],
            //         ValidAudience = configuration["JwtSettings:Audience"],
            //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
            //     };
            // });
           
            //IConfigurationSection _IConfigurationSection = configuration.GetSection("IdentityDefaultOptions");
            //services.Configure<DefaultIdentityOptions>(_IConfigurationSection);
            //var _DefaultIdentityOptions = _IConfigurationSection.Get<DefaultIdentityOptions>();
            //AddIdentityOptions.SetOptions(services, _DefaultIdentityOptions);

            services.AddTransient(typeof(IBaseRepo<>), typeof(BaseRepo<>));
            return services;
        }
    }
}
