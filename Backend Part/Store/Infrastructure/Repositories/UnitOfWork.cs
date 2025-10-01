using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;

        public UnitOfWork(StoreDbContext context) => _context = context;

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
