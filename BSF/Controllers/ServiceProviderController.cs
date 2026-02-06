
using Application.Servicess.ServiceProviderService.DTOs;
using Application.Servicess.ServiceProviderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;



namespace BSF.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceProviderController : ControllerBase
    {
        private readonly IServiceProviderService _serviceProviderService;
        public ServiceProviderController(IServiceProviderService serviceProviderService)
        {
            _serviceProviderService = serviceProviderService;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterServiceProvider(ServiceProviderRegistrationRequest request)
        {
            await _serviceProviderService.ServiceProviderRegistration(request);
            return Ok();
        }
        [Authorize(Roles ="ServiceProvider")]
        [HttpGet("MyAccount")]
        public async Task<IActionResult> GetMyAccount()
        {
            var GetServiceProviderAccount=await _serviceProviderService.GetServiceProviderAccount();
            return Ok(GetServiceProviderAccount);
        }
        [Authorize(Roles ="ServiceProvider")]
        [HttpPost("UpdateMyAccount")]
        public async Task<IActionResult> UpdateMyAccount(ServiceProviderRegistrationRequest request)
        {
            await _serviceProviderService.UpdateServiceProviderAccount(request);
            return Ok();
        }
    }
}