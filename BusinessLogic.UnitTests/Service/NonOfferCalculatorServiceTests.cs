using System.Collections.Generic;
using BusinessLogic.Service;
using Domain.Entity;
using Domain.Interface.Service;
using NUnit.Framework;

namespace BusinessLogic.UnitTests.Service
{
    [TestFixture]
    public class NonOfferCalculatorServiceTests
    {
        private INonOfferCalculatorService _nonOfferCalculatorService;

        [SetUp]
        public void Setup()
        {
            _nonOfferCalculatorService = new NonOfferCalculatorService();
        }

        [Test]
        public void Calculate_WhenNoProducts_Returns0()
        {
            const decimal expected = 0m;
            var products = new List<Product>();

            var actual = _nonOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Calculate_WhenNothingIsProcessed_ReturnsCostOfAllProducts()
        {
            const decimal expected = 11.2m;
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m}
            };

            var actual = _nonOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Calculate_WhenSomeProductsAreProcessed_ReturnsCostOfTheUnprocessedProducts()
        {
            const decimal expected = 7.6m;
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m, Processed = true},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m, Processed = true},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m, Processed = true},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m, Processed = true},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m, Processed = true}
            };

            var actual = _nonOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
