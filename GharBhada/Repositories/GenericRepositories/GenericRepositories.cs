using GharBhada.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace GharBhada.Repositories.GenericRepositories
{
    public class GenericRepositories : IGenericRepositories
    {
        private readonly GharBhadaContext _context;

        public GenericRepositories(GharBhadaContext context)
        {
            _context = context;
        }

        public async Task<List<T>> SelectAll<T>() where T : class
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> SelectAll<T>(Expression<Func<T, bool>> deleted) where T : class
        {
            return await _context.Set<T>().Where(deleted).ToListAsync();
        }
        public async Task<T?> SelectbyId<T>(int id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task UpdatebyId<T>(int id, T instance) where T : class
        {
            _context.Set<T>().Update(instance);
            await _context.SaveChangesAsync();
        }

        public async Task Create<T>(T instance) where T : class
        {
            _context.Set<T>().Add(instance);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> SelectAllInclude<T, K>(Expression<Func<T, bool>> deleted, Expression<Func<T, K>> include)
            where T : class
        {
            return await _context.Set<T>().Where(deleted).Include(include).ToListAsync();
        }

        public async Task DeleteById<T>(int id) where T : class
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }
    }
}
