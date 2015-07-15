using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Service;
using Domain.Entity;
using Domain.Interface.Repository;
using Domain.Interface.Service;
using Moq;
using NUnit.Framework;

namespace BusinessLogic.UnitTests.Service
{
    [TestFixture]
    public class DiscountOfferCalculatorServiceTests
    {
        private Mock<IDiscountOfferRepository> _discountOfferRepositoryMock;
        private IDiscountOfferCalculatorService _discountOfferCalculatorService;

        [SetUp]
        public void Setup()
        {
            _discountOfferRepositoryMock = new Mock<IDiscountOfferRepository>();
            _discountOfferCalculatorService = new DiscountOfferCalculatorService(_discountOfferRepositoryMock.Object);
        }

        [Test]
        public void Calculate_WhenNoDiscountsAvailable_Returns0()
        {
            const decimal expected = 0m;
            var products = new List<Product> {new Product {Id = 1, Name = "Test Product", UnitPrice = 1.00m}};

            _discountOfferRepositoryMock.Setup(m => m.GetDiscountOffers()).Returns(new List<DiscountOffer>());

            var actual = _discountOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(products.All(p => p.Processed == false));
        }

        [Test]
        public void Calculate_WhenNoDiscountApplies_Returns0()
        {
            const decimal expected = 0m;
            var products = new List<Product> { new Product { Id = 1, Name = "Test Product", UnitPrice = 1.00m } };

            _discountOfferRepositoryMock.Setup(m => m.GetDiscountOffers()).Returns(new List<DiscountOffer>
                {
                    new DiscountOffer { ProductBoughtId = 2, NumberToBuy = 2, ProductDiscountedId = 3, PercentageOff = 0.5m }
                });

            var actual = _discountOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(products.All(p => p.Processed == false));
        }

        [Test]
        public void Calculate_WhenNoProducts_Returns0()
        {
            const decimal expected = 0m;
            var products = new List<Product>();

            _discountOfferRepositoryMock.Setup(m => m.GetDiscountOffers()).Returns(new List<DiscountOffer>
                {
                    new DiscountOffer { ProductBoughtId = 2, NumberToBuy = 2, ProductDiscountedId = 3, PercentageOff = 0.5m }
                });

            var actual = _discountOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Calculate_WhenNoProductsAndNoDiscounts_Returns0()
        {
            const decimal expected = 0m;
            var products = new List<Product>();

            _discountOfferRepositoryMock.Setup(m => m.GetDiscountOffers()).Returns(new List<DiscountOffer>());

            var actual = _discountOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Calculate_When1DiscountAppliesOnce_ReturnsCorrectCost()
        {
            const decimal expected = 0.5m;
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m}
            };

            _discountOfferRepositoryMock.Setup(m => m.GetDiscountOffers()).Returns(new List<DiscountOffer>
                {
                    new DiscountOffer { ProductBoughtId = 1, NumberToBuy = 2, ProductDiscountedId = 3, PercentageOff = 0.5m }
                });

            var actual = _discountOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(products.Where(p => p.Name == "Bread").All(p => p.Processed));
            Assert.That(products.Where(p => p.Name == "Butter").All(p => p.Processed == false));
        }

        [Test]
        public void Calculate_When1DiscountAppliesMoreThanOnce_ReturnsCorrectCost()
        {
            const decimal expected = 1m;
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m}
            };

            _discountOfferRepositoryMock.Setup(m => m.GetDiscountOffers()).Returns(new List<DiscountOffer>
                {
                    new DiscountOffer { ProductBoughtId = 1, NumberToBuy = 2, ProductDiscountedId = 3, PercentageOff = 0.5m }
                });

            var actual = _discountOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(products.Where(p => p.Name == "Bread").All(p => p.Processed));
            Assert.That(products.Where(p => p.Name == "Butter").All(p => p.Processed == false));
        }

        [Test]
        public void Calculate_When1DiscountAppliesMoreThanOnceAndThereAreAdditionalDiscountableItems_ReturnsCorrectCost()
        {
            const decimal expected = 1m;
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

            _discountOfferRepositoryMock.Setup(m => m.GetDiscountOffers()).Returns(new List<DiscountOffer>
                {
                    new DiscountOffer { ProductBoughtId = 1, NumberToBuy = 2, ProductDiscountedId = 3, PercentageOff = 0.5m }
                });

            var actual = _discountOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(products.Count(p => p.Name == "Bread" && p.Processed) == 2);
            Assert.That(products.Count(p => p.Name == "Bread" && p.Processed == false) == 6);
            Assert.That(products.Where(p => p.Name == "Butter").All(p => p.Processed == false));
        }

        [Test]
        public void Calculate_When1DiscountAppliesMoreThanOnceAndThereAreAdditionalDiscountGeneratingItems_ReturnsCorrectCost()
        {
            const decimal expected = 1m;
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m}
            };

            _discountOfferRepositoryMock.Setup(m => m.GetDiscountOffers()).Returns(new List<DiscountOffer>
                {
                    new DiscountOffer { ProductBoughtId = 1, NumberToBuy = 2, ProductDiscountedId = 3, PercentageOff = 0.5m }
                });

            var actual = _discountOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(products.Where(p => p.Name == "Bread").All(p => p.Processed));
            Assert.That(products.Where(p => p.Name == "Butter").All(p => p.Processed == false));
        }

        [Test]
        public void Calculate_When1DiscountAppliesMoreThanOnceAndThereAreAdditionalDiscountableAndDiscountGeneratingItems_ReturnsCorrectCost()
        {
            const decimal expected = 1m;
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m}
            };

            _discountOfferRepositoryMock.Setup(m => m.GetDiscountOffers()).Returns(new List<DiscountOffer> 
            { new DiscountOffer { ProductBoughtId = 1, NumberToBuy = 2, ProductDiscountedId = 3, PercentageOff = 0.5m } });

            var actual = _discountOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(products.Count(p => p.Name == "Bread" && p.Processed) == 2);
            Assert.That(products.Count(p => p.Name == "Bread" && p.Processed == false) == 1);
            Assert.That(products.Where(p => p.Name == "Butter").All(p => p.Processed == false));
        }

        [Test]
        public void Calculate_WhenThereAreMultipleDiscounts_ReturnsCorrectCost()
        {
            const decimal expected = 2.79m;
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 2, Name = "Milk", UnitPrice = 1.15m},
                new Product {Id = 2, Name = "Milk", UnitPrice = 1.15m},
                new Product {Id = 2, Name = "Milk", UnitPrice = 1.15m},
                new Product {Id = 2, Name = "Milk", UnitPrice = 1.15m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 5, Name = "Bacon", UnitPrice = 1.50m},
                new Product {Id = 5, Name = "Bacon", UnitPrice = 1.50m},
                new Product {Id = 5, Name = "Bacon", UnitPrice = 1.50m},
                new Product {Id = 5, Name = "Bacon", UnitPrice = 1.50m},
                new Product {Id = 6, Name = "Bananas", UnitPrice = 0.7m},
                new Product {Id = 6, Name = "Bananas", UnitPrice = 0.7m},
                new Product {Id = 6, Name = "Bananas", UnitPrice = 0.7m},
                new Product {Id = 7, Name = "Peaches", UnitPrice = 1m},
                new Product {Id = 7, Name = "Peaches", UnitPrice = 1m},
                new Product {Id = 8, Name = "Table Salt", UnitPrice = 0.5m}
            };

            _discountOfferRepositoryMock.Setup(m => m.GetDiscountOffers()).Returns(new List<DiscountOffer>
                {
                    new DiscountOffer { ProductBoughtId = 1, NumberToBuy = 2, ProductDiscountedId = 5, PercentageOff = 0.2m },
                    new DiscountOffer { ProductBoughtId = 2, NumberToBuy = 3, ProductDiscountedId = 6, PercentageOff = 0.8m },
                    new DiscountOffer { ProductBoughtId = 3, NumberToBuy = 5, ProductDiscountedId = 7, PercentageOff = 0.3m },
                    new DiscountOffer { ProductBoughtId = 4, NumberToBuy = 6, ProductDiscountedId = 8, PercentageOff = 0.5m }
                });

            var actual = _discountOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(products.Count(p => p.Name == "Bacon" && p.Processed) == 2);
            Assert.That(products.Count(p => p.Name == "Bacon" && p.Processed == false) == 2);
            Assert.That(products.Count(p => p.Name == "Bananas" && p.Processed) == 1);
            Assert.That(products.Count(p => p.Name == "Bananas" && p.Processed == false) == 2);
            Assert.That(products.Where(p => p.Name == "Peaches").All(p => p.Processed == false));
            Assert.That(products.Where(p => p.Name == "Table Salt").All(p => p.Processed));
            Assert.That(products
                .Where(p => p.Name == "Butter" || p.Name == "Milk" || p.Name == "Bread" || p.Name == "Cheese")
                .All(p => p.Processed == false));
        }

        [Test]
        public void Calculate_WhenProductsAreAllJumbledUp_ReturnsCorrectCost()
        {
            const decimal expected = 2.79m;
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 2, Name = "Milk", UnitPrice = 1.15m},
                new Product {Id = 2, Name = "Milk", UnitPrice = 1.15m},
                new Product {Id = 8, Name = "Table Salt", UnitPrice = 0.5m},
                new Product {Id = 2, Name = "Milk", UnitPrice = 1.15m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 7, Name = "Peaches", UnitPrice = 1m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 2, Name = "Milk", UnitPrice = 1.15m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 6, Name = "Bananas", UnitPrice = 0.7m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 6, Name = "Bananas", UnitPrice = 0.7m},
                new Product {Id = 3, Name = "Bread", UnitPrice = 1.00m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 7, Name = "Peaches", UnitPrice = 1m},
                new Product {Id = 5, Name = "Bacon", UnitPrice = 1.50m},
                new Product {Id = 5, Name = "Bacon", UnitPrice = 1.50m},
                new Product {Id = 5, Name = "Bacon", UnitPrice = 1.50m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
                new Product {Id = 5, Name = "Bacon", UnitPrice = 1.50m},
                new Product {Id = 6, Name = "Bananas", UnitPrice = 0.7m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m}
            };

            _discountOfferRepositoryMock.Setup(m => m.GetDiscountOffers()).Returns(new List<DiscountOffer>
                {
                    new DiscountOffer { ProductBoughtId = 1, NumberToBuy = 2, ProductDiscountedId = 5, PercentageOff = 0.2m },
                    new DiscountOffer { ProductBoughtId = 2, NumberToBuy = 3, ProductDiscountedId = 6, PercentageOff = 0.8m },
                    new DiscountOffer { ProductBoughtId = 3, NumberToBuy = 5, ProductDiscountedId = 7, PercentageOff = 0.3m },
                    new DiscountOffer { ProductBoughtId = 4, NumberToBuy = 6, ProductDiscountedId = 8, PercentageOff = 0.5m }
                });

            var actual = _discountOfferCalculatorService.Calculate(products);

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(products.Count(p => p.Name == "Bacon" && p.Processed) == 2);
            Assert.That(products.Count(p => p.Name == "Bacon" && p.Processed == false) == 2);
            Assert.That(products.Count(p => p.Name == "Bananas" && p.Processed) == 1);
            Assert.That(products.Count(p => p.Name == "Bananas" && p.Processed == false) == 2);
            Assert.That(products.Where(p => p.Name == "Peaches").All(p => p.Processed == false));
            Assert.That(products.Where(p => p.Name == "Table Salt").All(p => p.Processed));
            Assert.That(products
                .Where(p => p.Name == "Butter" || p.Name == "Milk" || p.Name == "Bread" || p.Name == "Cheese")
                .All(p => p.Processed == false));
        }
    }
}
