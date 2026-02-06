
namespace Application.Repositories{
    public interface IGenericRepository<T> where T: class
    {
        Task <T> GetBYIdAsync(int id);
        IQueryable<T> GetAll();
        Task InsertAsync(T entity);
        void Update(T entity);
        void Remove(T entity);

        Task<int> SaveChangesAsync();
        Task<IEnumerable<object>> GetAllAsync();
    }
}