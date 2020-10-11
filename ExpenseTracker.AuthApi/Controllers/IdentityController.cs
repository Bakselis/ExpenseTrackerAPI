using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExpenseTracker.AuthApi.Contracts.V1.Requests;
using ExpenseTracker.AuthApi.Contracts.V1.Responses;
using ExpenseTracker.AuthApi.Services;
using ExpenseTracker.Contracts.V1;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.AuthApi.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        
        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        
        /// <summary>
        /// Register to the system
        /// </summary>
        [HttpPost(ApiRoutes.Identity.Register)]
        [ProducesResponseType(typeof(AuthSuccessResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(AuthFailedResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }
            
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }
            
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
        
        /// <summary>
        /// Login to the system
        /// </summary>
        [HttpPost(ApiRoutes.Identity.Login)]
        [ProducesResponseType(typeof(AuthSuccessResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(AuthFailedResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }
            
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
        
        /// <summary>
        /// Refresh the JWT token by providing expired token and refresh token
        /// </summary>
        [HttpPost(ApiRoutes.Identity.Refresh)]
        [ProducesResponseType(typeof(AuthSuccessResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(AuthFailedResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authResponse = await _identityService.RefreshTokenAsync(request.Token, request.RefreshToken);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }
            
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken
            });
        }
    }
}