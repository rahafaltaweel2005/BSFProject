using Application.Generic_DTOs;
using Application.Services.Service;
using Application.Services.Service.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;

namespace BSF.Controllers
{
    [Authorize(Roles = "ServiceProvider")]

    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;
        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }
        [HttpPost("CreateService")]
        public async Task<IActionResult> CreateService([FromBody] SaveServiceRequest request)
        {
            await _servicesService.CreateService(request);
            return Ok();
        }
        [HttpPost("UpdateService")]
        public async Task<IActionResult> UpdateService(int id, [FromBody] SaveServiceRequest request)
        {
            await _servicesService.UpdateService(id, request);
            return Ok();
        }
        [HttpPost("DeleteService")]
        public async Task<IActionResult> DeleteService(int id)
        {
            await _servicesService.DeleteService(id);
            return Ok();
        }
        [HttpPost("GetMyServices")]
        public async Task<IActionResult> GetMyServices([FromBody] PaginationRequest request)
        {
            var response = await _servicesService.GetMyServices(request);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("GetAllServices")]
        public async Task<IActionResult> GetAllServices([FromBody] PaginationRequest request)
        {
            var response = await _servicesService.GetAllServices(request);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("ServicesbyCategory")]
        public async Task<IActionResult> GetServicesbyCategory([FromBody] PaginationRequest request)
        {
            var response = await _servicesService.GetServicesbyCategory(request);
            return Ok(response);
        }
    }
}