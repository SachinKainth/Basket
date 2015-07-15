using System.Collections.Generic;
using DataAccess.Repository;
using Domain.Entity;
using Domain.Interface.Repository;
using NUnit.Framework;

namespace DataAccess.IntegrationTests.Repository
{
    [TestFixture]
    public class DiscountOfferRepositoryTests
    {
        [Test]
        public void GetDiscountOffers_WhenCalled_ReturnsAllDiscountOffers()
        {
            var expected = new List<DiscountOffer>
                {
                    new DiscountOffer { ProductBoughtId = 1, NumberToBuy = 2, ProductDiscountedId = 3, PercentageOff = 0.5m }
                };

            IDiscountOfferRepository repo = new DiscountOfferRepository();
            var actual = repo.GetDiscountOffers();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
