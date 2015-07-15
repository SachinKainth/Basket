using DataAccess.Repository;
using Domain.Entity;
using Domain.Exception;
using Domain.Interface.Repository;
using NUnit.Framework;

namespace DataAccess.IntegrationTests.Repository
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private IProductRepository repo;

        [SetUp]
        public void Setup()
        {
            repo = new ProductRepository();
        }

        [Test]
        public void GetProduct_WhenExists_ReturnsThatProduct()
        {
            var expected = new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m};

            var actual = repo.GetProduct(3);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetProduct_WhenDoesntExists_ThrowsException()
        {
            Assert.Throws<ProductNotFoundException>(() => repo.GetProduct(4));
        }
    }
}
