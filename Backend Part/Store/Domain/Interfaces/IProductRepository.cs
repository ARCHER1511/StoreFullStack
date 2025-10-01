using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        //For specific methods related to Product entity
    }
}
