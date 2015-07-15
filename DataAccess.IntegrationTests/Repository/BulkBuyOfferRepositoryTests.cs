using System.Collections.Generic;
using DataAccess.Repository;
using Domain.Entity;
using Domain.Interface.Repository;
using NUnit.Framework;

namespace DataAccess.IntegrationTests.Repository
{
    [TestFixture]
    public class BulkBuyOfferRepositoryTests
    {
        [Test]
        public void GetBulkBuyOffers_WhenCalled_ReturnsAllBulkBuyOffers()
        {
            var expected = new List<BulkBuyOffer>
                {
                    new BulkBuyOffer { ProductId = 2, NumberToBuy = 3, NumberFree = 1 }
                };

            IBulkBuyOfferRepository repo = new BulkBuyOfferRepository();
            var actual = repo.GetBulkBuyOffers();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}