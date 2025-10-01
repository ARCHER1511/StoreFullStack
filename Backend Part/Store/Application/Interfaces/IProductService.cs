using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(string id);
        Task<Product> CreateAsync(ProductCreateUpdateDto dto);
        Task<Product?> UpdateAsync(string id, ProductCreateUpdateDto dto);
        Task DeleteAsync(string id);
    }
}
