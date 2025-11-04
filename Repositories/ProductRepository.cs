using CatalogService.Interfaces;
using CatalogService.Models;
using MongoDB.Driver;

namespace CatalogService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(IMongoDatabase database)
        {
            _products = database.GetCollection<Product>("Products");
        }

        public IEnumerable<Product> GetAll()
        {
            return _products.Find(_ => true).ToList();
        }
        public Product? GetById(Guid id)
        {
            return _products.Find(p => p.id == id).FirstOrDefault();
        }

        public Product Create(Product product)
        {
            if (product.id == Guid.Empty)
            {
                product.id = Guid.NewGuid();
            }

            // Ensure offers have IDs if not provided
            if (product.offers != null)
            {
                foreach (var offer in product.offers.Where(o => o.id == Guid.Empty))
                {
                    offer.id = Guid.NewGuid();
                }
            }

            _products.InsertOne(product);
            return product;
        }

        public Product? Update(Guid id, Product product)
        {
            var existingProduct = GetById(id);
            if (existingProduct == null)
            {
                return null;
            }

            product.id = id; // Ensure the ID stays the same
            var result = _products.ReplaceOne(p => p.id == id, product);
            
            return result.ModifiedCount > 0 ? product : null;
        }

        public bool Delete(Guid id)
        {
            var result = _products.DeleteOne(p => p.id == id);
            return result.DeletedCount > 0;
        }
    }
}
