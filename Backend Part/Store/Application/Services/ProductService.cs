using Application.DTOs;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductCodeProvider _codeProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _uploadsFolder;

        public ProductService(IProductRepository productRepo, IProductCodeProvider codeProvider, IUnitOfWork unitOfWork)
        {
            _productRepo = productRepo;
            _codeProvider = codeProvider;
            _unitOfWork = unitOfWork;
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");

            if (!Directory.Exists(_uploadsFolder))
                Directory.CreateDirectory(_uploadsFolder);
        }

        public async Task<IEnumerable<Product>> GetAllAsync() => await _productRepo.GetAllAsync();
        public async Task<Product?> GetByIdAsync(string id) => await _productRepo.GetByIdAsync(id);

        public async Task<Product> CreateAsync(ProductCreateUpdateDto dto)
        {
            var code = await CodeGenerator.GenerateNextAvailableCodeAsync(_codeProvider);

            var product = new Product
            {
                Category = dto.Category,
                Name = dto.Name,
                Price = dto.Price,
                MinimumQuantity = dto.MinimumQuantity,
                DiscountRate = dto.DiscountRate
            };

            if (dto.Image != null && dto.Image.Length > 0)
            {
                product.ImagePath = await SaveImageAsync(dto.Image);
            }

            product.SetProductCode(code);

            await _productRepo.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(string id, ProductCreateUpdateDto dto)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return null;

            product.Category = dto.Category;
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.MinimumQuantity = dto.MinimumQuantity;
            product.DiscountRate = dto.DiscountRate;

            // Handle image update
            if (dto.Image != null && dto.Image.Length > 0)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImagePath);
                    if (File.Exists(oldFilePath)) File.Delete(oldFilePath);
                }

                // Save new image
                product.ImagePath = await SaveImageAsync(dto.Image);
            }

            _productRepo.Update(product);
            await _unitOfWork.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(string id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return;

            // Delete image from disk
            if (!string.IsNullOrEmpty(product.ImagePath))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImagePath);
                if (File.Exists(filePath)) File.Delete(filePath);
            }

            _productRepo.Remove(product);
            await _unitOfWork.SaveChangesAsync();
        }

        // Helper method to save image
        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{image.FileName}";
            var filePath = Path.Combine(_uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"images/products/{uniqueFileName}";
        }
    }
}
