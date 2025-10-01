using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class StoreDbContextFactory : IDesignTimeDbContextFactory<StoreDbContext>
    {
        public StoreDbContext CreateDbContext(string[] args) 
        {
            var builder = new DbContextOptionsBuilder<StoreDbContext>();
            var connection = "Server=.;Database=StoreDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
            builder.UseSqlServer(connection);

            return new StoreDbContext(builder.Options);
        }
    }
}
