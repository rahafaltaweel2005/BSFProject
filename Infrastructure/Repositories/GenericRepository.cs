using Application.Repositories;
using  Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories{
    public class GenericRepository<T>: IGenericRepository<T> where T: class
    {
        private readonly BSFContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(BSFContext context){
            _context =context;
            _dbSet = _context.Set<T>();
        }
       public async Task<T> GetBYIdAsync(int id){
            return await _dbSet.FindAsync(id);
        }
        public IQueryable<T> GetAll(){
            return _dbSet;
        }
        public async Task InsertAsync(T entity){
            await _dbSet.AddAsync(entity);
        }
        public void Update(T entity){
            _dbSet.Update(entity);
        }
        public void  Delete(T entity){
            _dbSet.Remove(entity);
        }
        public async Task<int> SaveChangesAsync(){
            return await  _context.SaveChangesAsync();
        }

        public Task<IEnumerable<object>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}