using Auth.Controllers.Resources;
using Auth.Core.Security.Tokens;
using Auth.Core.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IMapper mapper, IAuthenticationService authenticationService)
        {
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserCredentialResource userCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authenticationService.CreateAccessTokenAsync(userCredentials.Email, userCredentials.Password);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            var accessTokenResource = _mapper.Map<AccessToken, AccessTokenResource>(response.Token);
            return Ok(accessTokenResource);
        }

        [HttpGet]
        [Route("healthy")]
        public async Task<IActionResult> CheckHealthStatus()
        {
            await Task.CompletedTask;
            return Ok("Healthy");
        }
    }
}
