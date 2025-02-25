using System.Linq.Expressions;

namespace GharBhada.Repositories.GenericRepositories
{
    public interface IGenericRepositories
    {
        Task<List<T>> SelectAll<T>() where T : class;
        Task<List<T>> SelectAll<T>(Expression<Func<T, bool>> deleted) where T : class;

        Task<List<T>> SelectAllInclude<T, K>(Expression<Func<T, bool>> deleted, Expression<Func<T, K>> include)
            where T : class;

        Task<T?> SelectbyId<T>(int id) where T : class;
        Task UpdatebyId<T>(int id, T instance) where T : class;
        Task Create<T>(T instance) where T : class;
        Task DeleteById<T>(int id) where T : class;
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
    }
}
