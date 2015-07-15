using System.Collections.Generic;
using Domain.Entity;
using Domain.Interface.Repository;

namespace DataAccess.Repository
{
    public class BulkBuyOfferRepository : IBulkBuyOfferRepository
    {
        public IList<BulkBuyOffer> GetBulkBuyOffers()
        {
            return new List<BulkBuyOffer>
            {
                new BulkBuyOffer { ProductId = 2, NumberToBuy = 3, NumberFree = 1 }
            };
        }
    }
}