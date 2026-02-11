using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Service
    {
        public int Id { get; set; }
        [StringLength(300)]
        public string Name { get; set; }
        public int ServiceProviderId { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
    }
}