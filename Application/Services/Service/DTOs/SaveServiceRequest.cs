using Microsoft.AspNetCore.Http;

namespace Application.Services.Service.DTOs
{
    public class SaveServiceRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public IFormFile? Image { get; set; }
        public bool DeleteImage{ get; set; }
    }
}