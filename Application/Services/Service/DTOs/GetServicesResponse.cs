using Microsoft.AspNetCore.Http;

namespace Application.Services.Service.DTOs
{
    public class GetServicesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
         public string? Image { get; set; }
        public bool DeleteImage{ get; set; }
    }
}