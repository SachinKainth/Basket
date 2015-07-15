using System.Collections.Generic;
using Domain.Entity;
using Domain.Interface.Repository;

namespace DataAccess.Repository
{
    public class DiscountOfferRepository : IDiscountOfferRepository
    {
        public IList<DiscountOffer> GetDiscountOffers()
        {
            return new List<DiscountOffer>
            {
                new DiscountOffer { ProductBoughtId = 1, NumberToBuy = 2, ProductDiscountedId = 3, PercentageOff = 0.5m }
            };
        }
    }
}