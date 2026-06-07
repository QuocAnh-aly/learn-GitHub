using MyApp.DTO;
using MyApp.Entities;
using MyApp.Repository;

namespace MyApp.Services
{
    public class ProductService : IProductSevice{
        private readonly IProductRepository _repo;
        public ProductService(IProductRepository repo){
            _repo = repo;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var list = await_repo.GetAllAsync();
            return list.Select(p => new ProductDto {
                Id = p.Id,
                Name
                \
                
            })
        }
    }
}