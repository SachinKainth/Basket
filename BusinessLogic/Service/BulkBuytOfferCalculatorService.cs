using System.Collections.Generic;
using System.Linq;
using Domain.Entity;
using Domain.Exception;
using Domain.Interface.Repository;
using Domain.Interface.Service;

namespace BusinessLogic.Service
{
    public class BulkBuytOfferCalculatorService : IBulkBuyOfferCalculatorService
    {
        private readonly IBulkBuyOfferRepository _bulkBuyOfferRepository;

        public BulkBuytOfferCalculatorService(IBulkBuyOfferRepository bulkBuyOfferRepository)
        {
            _bulkBuyOfferRepository = bulkBuyOfferRepository;
        }

        public void ProcessFreeProducts(IList<Product> products)
        {
            var offers = _bulkBuyOfferRepository.GetBulkBuyOffers();

            foreach (var offer in offers)
            {
                var applicableProducts = products.Where(product => product.Id == offer.ProductId).ToList();

                var numBought = applicableProducts.Count();

                int numDiscounts = numBought / (offer.NumberToBuy + offer.NumberFree);
                int totalFree = numDiscounts * offer.NumberFree;

                int nonQualifyingProductsCount = numBought - (numDiscounts * (offer.NumberToBuy + offer.NumberFree));
                if (nonQualifyingProductsCount >= offer.NumberToBuy)
                {
                    foreach (var product in products)
                    {
                        product.Processed = false;
                    }
                    throw new InsufficientFreeProductsSelectedException();
                }

                for (var i = 0; i < totalFree && i < applicableProducts.Count; i++)
                {
                    var applicableProduct = applicableProducts[i];
                    applicableProduct.Processed = true;
                }
            }
        }
    }
}