using System.Collections.Generic;
using System.Linq;
using Domain.Entity;
using Domain.Exception;
using Domain.Interface.Repository;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IList<Product> _products = new List<Product>
            {
                new Product { Id = 1, Name = "Butter", UnitPrice = 0.80m },
                new Product { Id = 2, Name = "Milk", UnitPrice = 1.15m },
                new Product { Id = 3, Name = "Bread", UnitPrice = 1.00m }
            };

        public Product GetProduct(int productId)
        {
            var product = _products.SingleOrDefault(p => p.Id == productId);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            return product;
        }
    }
}