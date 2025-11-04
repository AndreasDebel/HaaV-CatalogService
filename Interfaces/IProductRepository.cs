using CatalogService.Models;

namespace CatalogService.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product? GetById(Guid id);
        Product Create(Product product);
        Product? Update(Guid id, Product product);
        bool Delete(Guid id);
    }
}
