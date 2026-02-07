using Domain.Enums;

namespace Application.Servicess.LookupService
{
    public class GetLookupResponse
    {
         public int Id { get; set; }
        public string Name { get; set; }
        public ServiceCategoryEnum Code { get; set; }
        
    }
}