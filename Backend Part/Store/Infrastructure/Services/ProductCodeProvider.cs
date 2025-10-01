using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProductCodeProvider : IProductCodeProvider 
    {
        private readonly StoreDbContext _db;
        public ProductCodeProvider(StoreDbContext db) 
        {
            _db = db;
        }
        public async Task<int> GetCurrentMaxNumberAsync()
        {
            // Load only the ProductCode column into memory first,
            // then parse it in LINQ-to-Objects (EF can translate ToListAsync).
            var codes = await _db.Products
                .Select(p => p.ProductCode)
                .Where(c => c.StartsWith("P"))
                .ToListAsync();

            return codes
                .Select(c =>
                {
                    // safely strip 'P' and parse
                    return int.TryParse(c.Substring(1), out var n) ? n : 0;
                })
                .DefaultIfEmpty(0)
                .Max();
        }

        public async Task<int?> GetNextAvailableNumberAsync()
        {
            var codes = await _db.Products
                .Select(p => p.ProductCode)
                .Where(c => c.StartsWith("P"))
                .ToListAsync();

            var numbers = codes
                .Select(c => int.TryParse(c.Substring(1), out var n) ? n : 0)
                .OrderBy(n => n)
                .ToList();

            int expected = 1;
            foreach (var n in numbers)
            {
                if (n > expected) return expected;
                expected++;
            }
            return expected;
        }
    }
}
