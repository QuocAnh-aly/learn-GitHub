using System;
using System.Collections.Generic;
using System.Text;
using MyApp.Entities;
using Microsoft.EntityFrameworkCore;
namespace MyApp.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product p);
        Task<bool> UpdateAsync(Product p);
        Task<bool> DeleteAsync(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db)
        {
            _db = db;
            
        }
        public async Task<List<Product>> GetAllAsync()
        {
            return await _db.Products.ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(int id) { 
            return await _db.Products.FindAsync(id);
        }
        public async Task<Product> AddAsync(Product p) {
            _db.Products.Add(p);
            await _db.SaveChangesAsync();
            return p;
        }
        public async Task<bool> UpdateAsync(Product p) {
            _db.Products.Update(p);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool > DeleteAsync(int id) {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return false;
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }

    }
}
