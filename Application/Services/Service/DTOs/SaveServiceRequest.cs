namespace Application.Services.Service.DTOs
{
    public class SaveServiceRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
    }
}