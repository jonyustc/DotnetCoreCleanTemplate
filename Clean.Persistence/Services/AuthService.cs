using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clean.Domain.Entities;
using Clean.Application.Bases;
using Clean.Application.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Clean.Application.Interfaces;

namespace Clean.Persistence.Services
{
    public class AuthService : BaseResponseHandler, IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly JwtSettings _jwtSettings;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_jwtSettings = jwtSettings;
        }

        public async Task<BaseResponse<LoginResult>> Login(LoginCommand request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return NotFound<LoginResult>("UserNotFound");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName??"", request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return NotFound<LoginResult>("InvalidCredentials");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            LoginResult response = new LoginResult
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName
            };

            return Success(response);
        }

        public async Task<BaseResponse<RegisterResult>> Register(RegisterCommand request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.UserName);

            if (existingUser != null)
            {
                return Conflict<RegisterResult>("UsernameExists");
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = true
            };

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);

            if (existingEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                    return Created(new RegisterResult() { UserId = user.Id });
                }
                else
                {
                    return BadRequest<RegisterResult>("BadRequestDetails", result.Errors.Select(a => a.Description).ToList());
                }
            }
            else
            {
                return Conflict<RegisterResult>("EmailExists");
            }
        }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(CustomClaimTypes.Uid, user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKeyForQMSApplication@BsRm"));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "https://localhost:7084/",
                audience: "https://localhost:7084/",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
