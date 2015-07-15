using System.Collections.Generic;
using Domain.Entity;

namespace Domain.Interface.Repository
{
    public interface IDiscountOfferRepository
    {
        IList<DiscountOffer> GetDiscountOffers();
    }
}