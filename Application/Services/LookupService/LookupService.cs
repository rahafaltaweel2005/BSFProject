
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.LookupService
{
    public class LookupService : ILookupService
    {
        private readonly IGenericRepository<ServiceCategory> _serviceCategoryRepo;
        public LookupService(IGenericRepository<ServiceCategory> serviceCategoryRepo)
        {
            _serviceCategoryRepo = serviceCategoryRepo;
        }
        public async Task<List<GetLookupResponse>> GetAllServiceCategories()
        {
            var categories = await _serviceCategoryRepo.GetAll()
                .Select(sc => new GetLookupResponse
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Code = sc.Code
                }).ToListAsync();

            return categories;
        }

        
    }
}