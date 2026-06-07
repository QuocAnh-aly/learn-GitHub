using System;
using System.Collections.Generic;
using System.Text;
using MyApp.DTO;
using Microsoft.EntityFrameworkCore;
namespace MyApp.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(CreateProductDto dto);
        Task<bool> UpdateAsync(int id, ProductDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
