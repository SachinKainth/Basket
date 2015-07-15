using System.Collections.Generic;
using System.Linq;
using Domain.Entity;
using Domain.Interface.Repository;
using Domain.Interface.Service;

namespace BusinessLogic.Service
{
    public class DiscountOfferCalculatorService : IDiscountOfferCalculatorService
    {
        private readonly IDiscountOfferRepository _discountOfferRepository;

        public DiscountOfferCalculatorService(IDiscountOfferRepository discountOfferRepository)
        {
            _discountOfferRepository = discountOfferRepository;
        }

        public decimal Calculate(IList<Product> products)
        {
            var totalCost = 0m;

            var discounts = _discountOfferRepository.GetDiscountOffers();

            foreach (var discount in discounts)
            {
                var numBought = products.Count(product => product.Id == discount.ProductBoughtId);
     
                int numDiscounts = numBought / discount.NumberToBuy;

                var productsToDiscount = products.Where(product => product.Id == discount.ProductDiscountedId).ToList();

                for (var i = 0; i < productsToDiscount.Count; i++)
                {
                    if (i == numDiscounts)
                    {
                        break;
                    }

                    var productToDiscount = productsToDiscount[i];

                    totalCost += productToDiscount.UnitPrice * (1 - discount.PercentageOff);
                    productToDiscount.Processed = true;
                }
            }

            return totalCost;
        }
    }
}