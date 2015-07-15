using System.Linq;
using BusinessLogic.Service;
using Domain.Entity;
using Domain.Exception;
using Domain.Interface.Repository;
using Moq;
using NUnit.Framework;

namespace BusinessLogic.UnitTests.Service
{
    [TestFixture]
    public class BasketServiceTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private BasketService _basketService;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _basketService = new BasketService(_productRepositoryMock.Object, null, null, null);
        }

        [Test]
        public void AddProduct_WhenCalledWithAQuantityOf1_Adds1OfThatProductToTheBasket()
        {
            _productRepositoryMock.Setup(m => m.GetProduct(It.IsAny<int>())).Returns(new Product { Id = 1, Name = "Test Product", UnitPrice = 1.00m });

            _basketService.AddProduct(1, 1);

            Assert.That(_basketService.Products.Count, Is.EqualTo(1));
            Assert.That(_basketService.Products[0].Name, Is.EqualTo("Test Product"));
        }

        [Test]
        public void AddProduct_WhenCalledWithAQuantityOfN_AddsNOfThatProductToTheBasket()
        {
            _productRepositoryMock.Setup(m => m.GetProduct(It.IsAny<int>())).Returns(new Product { Id = 1, Name = "Test Product", UnitPrice = 1.00m });

            _basketService.AddProduct(1, 10);

            Assert.That(_basketService.Products.Count, Is.EqualTo(10));
            Assert.That(_basketService.Products.All(p => p.Name == "Test Product"));
        }

        [Test]
        public void AddProduct_WhenCalledOnANonExistantProduct_ThrowsException()
        {
            _productRepositoryMock.Setup(m => m.GetProduct(It.IsAny<int>())).Throws<ProductNotFoundException>();

            Assert.Throws<ProductNotFoundException>(() => _basketService.AddProduct(1, 1));
        }
    }
}