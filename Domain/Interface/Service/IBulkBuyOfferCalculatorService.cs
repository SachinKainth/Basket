using System.Collections.Generic;
using Domain.Entity;

namespace Domain.Interface.Service
{
    public interface IBulkBuyOfferCalculatorService
    {
        void ProcessFreeProducts(IList<Product> products);
    }
}