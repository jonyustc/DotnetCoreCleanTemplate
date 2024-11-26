using Clean.Application.Bases;
using Clean.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Persistence
{
    public static class AddIdentityOptions
    {
        public static void SetOptions(IServiceCollection services, DefaultIdentityOptions _DefaultIdentityOptions)
        {
            try
            {
                services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = _DefaultIdentityOptions.PasswordRequireDigit;
                    options.Password.RequiredLength = _DefaultIdentityOptions.PasswordRequiredLength;
                    options.Password.RequireNonAlphanumeric = _DefaultIdentityOptions.PasswordRequireNonAlphanumeric;
                    options.Password.RequireUppercase = _DefaultIdentityOptions.PasswordRequireUppercase;
                    options.Password.RequireLowercase = _DefaultIdentityOptions.PasswordRequireLowercase;
                    options.Password.RequiredUniqueChars = _DefaultIdentityOptions.PasswordRequiredUniqueChars;

                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(_DefaultIdentityOptions.LockoutDefaultLockoutTimeSpanInMinutes);
                    options.Lockout.MaxFailedAccessAttempts = _DefaultIdentityOptions.LockoutMaxFailedAccessAttempts;
                    options.Lockout.AllowedForNewUsers = _DefaultIdentityOptions.LockoutAllowedForNewUsers;

                    options.User.RequireUniqueEmail = _DefaultIdentityOptions.UserRequireUniqueEmail;

                    options.SignIn.RequireConfirmedEmail = _DefaultIdentityOptions.SignInRequireConfirmedEmail;
                }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
