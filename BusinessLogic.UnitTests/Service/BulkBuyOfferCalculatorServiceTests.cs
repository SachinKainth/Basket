using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Service;
using Domain.Entity;
using Domain.Exception;
using Domain.Interface.Repository;
using Domain.Interface.Service;
using Moq;
using NUnit.Framework;

namespace BusinessLogic.UnitTests.Service
{
    [TestFixture]
    public class BulkBuyOfferCalculatorServiceTests
    {
        private Mock<IBulkBuyOfferRepository> _bulkBuyOfferRepositoryMock;
        private IBulkBuyOfferCalculatorService _bulkBuyOfferCalculatorService;

        [SetUp]
        public void Setup()
        {
            _bulkBuyOfferRepositoryMock = new Mock<IBulkBuyOfferRepository>();
            _bulkBuyOfferCalculatorService = new BulkBuytOfferCalculatorService(_bulkBuyOfferRepositoryMock.Object);
        }

        [Test]
        public void ProcessFreeProducts_WhenNoDiscountsAvailable_MarksNothing()
        {
            var products = new List<Product> {new Product {Id = 1, Name = "Test Product", UnitPrice = 1.00m}};

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>());

            _bulkBuyOfferCalculatorService.ProcessFreeProducts(products);

            Assert.That(products.All(p => p.Processed == false));
        }

        [Test]
        public void ProcessFreeProducts_WhenNoDiscountApplies_MarksNothing()
        {
            var products = new List<Product> {new Product {Id = 1, Name = "Test Product", UnitPrice = 1.00m}};

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
                {
                    new BulkBuyOffer { ProductId = 2, NumberToBuy = 1, NumberFree = 1 }
                });

            _bulkBuyOfferCalculatorService.ProcessFreeProducts(products);

            Assert.That(products.All(p => p.Processed == false));
        }

        [Test]
        public void ProcessFreeProducts_WhenNoProducts_MarksNothing()
        {
            var products = new List<Product>();

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
            {
                new BulkBuyOffer { ProductId = 2, NumberToBuy = 1, NumberFree = 1 }
            });

            _bulkBuyOfferCalculatorService.ProcessFreeProducts(products);
        }

        [Test]
        public void ProcessFreeProducts_WhenNoProductsAndNoDiscounts_MarksNothing()
        {
            var products = new List<Product>();

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>());

            _bulkBuyOfferCalculatorService.ProcessFreeProducts(products);
        }

        [Test]
        public void ProcessFreeProducts_When1DiscountAppliesOnce_MarkTheCorrectProducts()
        {
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m}
            };

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
            {
                new BulkBuyOffer { ProductId = 1, NumberToBuy = 1, NumberFree = 1 }
            });

            _bulkBuyOfferCalculatorService.ProcessFreeProducts(products);

            Assert.That(products.Count(p => p.Name == "Butter" && p.Processed) == 1);
            Assert.That(products.Count(p => p.Name == "Butter" && p.Processed == false) == 1);
        }

        [Test]
        public void ProcessFreeProducts_When1DiscountAppliesMoreThanOnce_MarkTheCorrectProducts()
        {
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m}
            };

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
            {
                new BulkBuyOffer { ProductId = 1, NumberToBuy = 1, NumberFree = 1 }
            });

            _bulkBuyOfferCalculatorService.ProcessFreeProducts(products);

            Assert.That(products.Count(p => p.Name == "Butter" && p.Processed) == 2);
            Assert.That(products.Count(p => p.Name == "Butter" && p.Processed == false) == 2);
        }

        [Test]
        public void ProcessFreeProducts_When1DiscountAppliesOnceAndTheUserHasNotSelectedAnyFreeProducts_MarksNothingAndThrowsException()
        {
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m}
            };

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
            {
                new BulkBuyOffer { ProductId = 1, NumberToBuy = 3, NumberFree = 2 }
            });

            Assert.Throws<InsufficientFreeProductsSelectedException>(() => _bulkBuyOfferCalculatorService.ProcessFreeProducts(products));
            Assert.That(products.All(p => p.Processed == false));
        }

        [Test]
        public void ProcessFreeProducts_When1DiscountAppliesOnceAndTheUserHasNotSelectedEnoughFreeProducts_MarksNothingAndThrowsException()
        {
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m}
            };

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
            {
                new BulkBuyOffer { ProductId = 1, NumberToBuy = 3, NumberFree = 2 }
            });

            Assert.Throws<InsufficientFreeProductsSelectedException>(() => _bulkBuyOfferCalculatorService.ProcessFreeProducts(products));
            Assert.That(products.All(p => p.Processed == false));
        }

        [Test]
        public void ProcessFreeProducts_When1DiscountAppliesMoreThanOnceAndTheUserHasNotSelectedAnyFreeProductsForFinalDiscountOpportunity_MarksNothingAndThrowsException()
        {
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m}
            };

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
            {
                new BulkBuyOffer { ProductId = 1, NumberToBuy = 3, NumberFree = 1 }
            });

            Assert.Throws<InsufficientFreeProductsSelectedException>(() => _bulkBuyOfferCalculatorService.ProcessFreeProducts(products));
            Assert.That(products.All(p => p.Processed == false));
        }

        [Test]
        public void ProcessFreeProducts_When1DiscountAppliesMoreThanOnceAndTheUserHasNotSelectedEnoughFreeProductsForFinalDiscountOpportunity_MarksNothingAndThrowsException()
        {
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m}
            };

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
            {
                new BulkBuyOffer { ProductId = 1, NumberToBuy = 2, NumberFree = 2 }
            });

            Assert.Throws<InsufficientFreeProductsSelectedException>(() => _bulkBuyOfferCalculatorService.ProcessFreeProducts(products));
            Assert.That(products.All(p => p.Processed == false));
        }

        [Test]
        public void ProcessFreeProducts_When1DiscountAppliesOnceAndTheUserHasNotSelectedEnoughQualifyingProducts_MarksNothing()
        {
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m}
            };

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
            {
                new BulkBuyOffer { ProductId = 1, NumberToBuy = 2, NumberFree = 2 }
            });

            _bulkBuyOfferCalculatorService.ProcessFreeProducts(products);

            Assert.That(products.All(p => p.Processed == false));
        }

        [Test]
        public void ProcessFreeProducts_When1DiscountAppliesMoreThanOnceAndTheUserHasNotSelectedEnoughQualifyingProductsForFinalDiscountOpportunity_MarkTheCorrectProducts()
        {
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m}
            };

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
            {
                new BulkBuyOffer { ProductId = 1, NumberToBuy = 2, NumberFree = 2 }
            });

            _bulkBuyOfferCalculatorService.ProcessFreeProducts(products);

            Assert.That(products.Count(p => p.Name == "Butter" && p.Processed) == 2);
            Assert.That(products.Count(p => p.Name == "Butter" && p.Processed == false) == 3);
        }
 
        [Test]
        public void ProcessFreeProducts_WhenThereAreMultipleDiscounts_MarkTheCorrectProducts()
        {
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
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

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
                {
                    new BulkBuyOffer { ProductId = 1, NumberToBuy = 2, NumberFree = 1 },
                    new BulkBuyOffer { ProductId = 2, NumberToBuy = 1, NumberFree = 2 },
                    new BulkBuyOffer { ProductId = 4, NumberToBuy = 3, NumberFree = 4 },
                    new BulkBuyOffer { ProductId = 6, NumberToBuy = 4, NumberFree = 2 },
                    new BulkBuyOffer { ProductId = 7, NumberToBuy = 1, NumberFree = 1 }
                });

            _bulkBuyOfferCalculatorService.ProcessFreeProducts(products);

            Assert.AreEqual(2, products.Count(p => p.Name == "Butter" && p.Processed));
            Assert.AreEqual(4, products.Count(p => p.Name == "Butter" && p.Processed == false));
            Assert.AreEqual(2, products.Count(p => p.Name == "Milk" && p.Processed));
            Assert.AreEqual(1, products.Count(p => p.Name == "Milk" && p.Processed == false));
            Assert.AreEqual(8, products.Count(p => p.Name == "Cheese" && p.Processed));
            Assert.AreEqual(6, products.Count(p => p.Name == "Cheese" && p.Processed == false));
            Assert.AreEqual(1, products.Count(p => p.Name == "Peaches" && p.Processed));
            Assert.AreEqual(1, products.Count(p => p.Name == "Peaches" && p.Processed == false));
            Assert.That(products
                .Where(p => p.Name == "Bread" || p.Name == "Bacon" || p.Name == "Bananas" || p.Name == "Table Salt")
                .All(p => p.Processed == false));
        }

        [Test]
        public void ProcessFreeProducts_WhenProductsAreAllJumbledUp_MarkTheCorrectProducts()
        {
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
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 4, Name = "Cheese", UnitPrice = 1.20m},
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

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
                {
                    new BulkBuyOffer { ProductId = 1, NumberToBuy = 2, NumberFree = 1 },
                    new BulkBuyOffer { ProductId = 2, NumberToBuy = 1, NumberFree = 2 },
                    new BulkBuyOffer { ProductId = 4, NumberToBuy = 3, NumberFree = 4 },
                    new BulkBuyOffer { ProductId = 6, NumberToBuy = 4, NumberFree = 2 },
                    new BulkBuyOffer { ProductId = 7, NumberToBuy = 1, NumberFree = 1 }
                });

            _bulkBuyOfferCalculatorService.ProcessFreeProducts(products);

            Assert.AreEqual(2, products.Count(p => p.Name == "Butter" && p.Processed));
            Assert.AreEqual(4, products.Count(p => p.Name == "Butter" && p.Processed == false));
            Assert.AreEqual(2, products.Count(p => p.Name == "Milk" && p.Processed));
            Assert.AreEqual(1, products.Count(p => p.Name == "Milk" && p.Processed == false));
            Assert.AreEqual(8, products.Count(p => p.Name == "Cheese" && p.Processed));
            Assert.AreEqual(6, products.Count(p => p.Name == "Cheese" && p.Processed == false));
            Assert.AreEqual(1, products.Count(p => p.Name == "Peaches" && p.Processed));
            Assert.AreEqual(1, products.Count(p => p.Name == "Peaches" && p.Processed == false));
            Assert.That(products
                .Where(p => p.Name == "Bread" || p.Name == "Bacon" || p.Name == "Bananas" || p.Name == "Table Salt")
                .All(p => p.Processed == false));
        }

        [Test]
        public void ProcessFreeProducts_WhenThereAreMultipleDiscountsAndTheUserHasNotSelectedAnyFreeProductsForOneProduct_MarksNothingAndThrowsException()
        {
            var products = new List<Product>
            {
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
                new Product {Id = 1, Name = "Butter", UnitPrice = 0.80m},
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
                new Product {Id = 8, Name = "Table Salt", UnitPrice = 0.5m}
            };

            _bulkBuyOfferRepositoryMock.Setup(m => m.GetBulkBuyOffers()).Returns(new List<BulkBuyOffer>
                {
                    new BulkBuyOffer { ProductId = 1, NumberToBuy = 2, NumberFree = 1 },
                    new BulkBuyOffer { ProductId = 2, NumberToBuy = 1, NumberFree = 2 },
                    new BulkBuyOffer { ProductId = 4, NumberToBuy = 3, NumberFree = 4 },
                    new BulkBuyOffer { ProductId = 6, NumberToBuy = 4, NumberFree = 2 },
                    new BulkBuyOffer { ProductId = 7, NumberToBuy = 1, NumberFree = 1 }
                });

            Assert.Throws<InsufficientFreeProductsSelectedException>(() => _bulkBuyOfferCalculatorService.ProcessFreeProducts(products));
            Assert.That(products.All(p => p.Processed == false));
        }
    }
}