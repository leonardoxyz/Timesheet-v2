using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Data
{
    public class Repository<T> where T : class
    {
        protected readonly Context _context;
        private readonly DbSet<T> _entities;

        public Repository(Context context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
