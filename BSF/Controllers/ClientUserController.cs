
using Application.Servicess.ClientUserService;
using Application.Servicess.ClientUserService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;



namespace BSF.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ClientUserController : ControllerBase
    {
        private readonly IClientUserService _clientUserService;
        public ClientUserController(IClientUserService clientUserService)
        {
            _clientUserService = clientUserService;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterClientUser(ClientUserRegistrationRequest request)
        {
            await _clientUserService.ClientUserRegistration(request);
            return Ok();
        }
        [Authorize(Roles ="User")]
        [HttpGet("MyAccount")]
        public async Task<IActionResult> GetMyAccount()
        {
            var GetClientUserAccount=await _clientUserService.GetClientUserAccount();
            return Ok(GetClientUserAccount);
        }
         [Authorize(Roles = "User")]
        [HttpPost("UpdateMyAccount")]
        public async Task<IActionResult> UpdateMyAccount([FromBody] ClientUserRegistrationRequest request)
        {
            await _clientUserService.UpdateClientUserAccount(request);
            return Ok();
        }
       

    }
}