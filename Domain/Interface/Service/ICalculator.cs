using System.Collections.Generic;
using Domain.Entity;

namespace Domain.Interface.Service
{
    public interface ICalculator
    {
        decimal Calculate(IList<Product> products);
    }
}