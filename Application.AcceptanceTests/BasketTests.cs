using BusinessLogic.Service;
using DataAccess.Repository;
using NUnit.Framework;

namespace Application.AcceptanceTests
{
    [TestFixture]
    public class BasketTests
    {
        private Basket _basket;

        [SetUp]
        public void Setup()
        {
            _basket = new Basket(new BasketService(
                new ProductRepository(),
                new DiscountOfferCalculatorService(new DiscountOfferRepository()),
                new BulkBuytOfferCalculatorService(new BulkBuyOfferRepository()),
                new NonOfferCalculatorService()));
        }

        [Test]
        public void GivenTheBasketHas1Bread1ButterAnd1Milk_WhenITotalTheBasket_ThenTheTotalShouldBe2Pounds95Pence()
        {
            decimal expected = 2.95m;

            _basket.AddProduct(1, 1);
            _basket.AddProduct(2, 1);
            _basket.AddProduct(3, 1);

            var actual = _basket.CalculateTotalCost();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenTheBasketHas2BreadAnd2Butter_WhenITotalTheBasket_ThenTheTotalShouldBe3Pounds10Pence()
        {
            decimal expected = 3.10m;

            _basket.AddProduct(1, 2);
            _basket.AddProduct(3, 2);

            var actual = _basket.CalculateTotalCost();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenTheBasketHas4Milk_WhenITotalTheBasket_ThenTheTotalShouldBe3Pounds45Pence()
        {
            decimal expected = 3.45m;

            _basket.AddProduct(2, 4);

            var actual = _basket.CalculateTotalCost();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenTheBasketHas1Bread2ButterAnd8Milk_WhenITotalTheBasket_ThenTheTotalShouldBe9Pounds()
        {
            decimal expected = 9.00m;

            _basket.AddProduct(1, 2);
            _basket.AddProduct(2, 8);
            _basket.AddProduct(3, 1);

            var actual = _basket.CalculateTotalCost();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}