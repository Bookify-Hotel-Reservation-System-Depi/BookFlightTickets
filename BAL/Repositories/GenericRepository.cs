using BLLProject.Interfaces;
using BLLProject.Specifications;
using DAL.Data;
using DAL.models;
using Microsoft.EntityFrameworkCore;
namespace BLLProject.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseClass
    {
        public readonly BookFilghtsDbContext _dbContext;
        public GenericRepository(BookFilghtsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
           await  _dbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);

        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (include != null)
            {
                query = include(query);
                return await query.ToListAsync();
            }
                

            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await SpecificationEvalutor<T>
                        .GetQuery(_dbContext.Set<T>(), spec)
                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await SpecificationEvalutor<T>
                        .GetQuery(_dbContext.Set<T>(), spec)
                        .AsNoTracking()
                        .ToListAsync();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }
    }
}
