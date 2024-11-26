using Clean.Application.Interfaces;
using Clean.Application.ViewModels;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Api.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            var response = await _authService.Login(request);   

            return CustomResult(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand request)
        {
            var response = await _authService.Register(request);
            return CustomResult(response);
        }
    }
}
