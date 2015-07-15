using System.Collections.Generic;
using System.Linq;
using Domain.Entity;
using Domain.Interface.Service;

namespace BusinessLogic.Service
{
    public class NonOfferCalculatorService : INonOfferCalculatorService
    {
        public decimal Calculate(IList<Product> products)
        {
            return products.Where(product => product.Processed == false).Sum(product => product.UnitPrice);
        }
    }
}