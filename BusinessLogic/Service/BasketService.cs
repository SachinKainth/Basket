using System.Collections.Generic;
using Domain.Entity;
using Domain.Interface.Repository;
using Domain.Interface.Service;

namespace BusinessLogic.Service
{
    public class BasketService : IBasketService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDiscountOfferCalculatorService _discountOfferCalculatorService;
        private readonly IBulkBuyOfferCalculatorService _bulkBuyOfferCalculatorService;
        private readonly INonOfferCalculatorService _nonOfferCalculatorService;

        public IList<Product> Products { get; private set; }
        
        public BasketService(
            IProductRepository productRepository, 
            IDiscountOfferCalculatorService discountOfferCalculatorService, 
            IBulkBuyOfferCalculatorService bulkBuyOfferCalculatorService, 
            INonOfferCalculatorService nonOfferCalculatorService)
        {
            _productRepository = productRepository;
            _discountOfferCalculatorService = discountOfferCalculatorService;
            _bulkBuyOfferCalculatorService = bulkBuyOfferCalculatorService;
            _nonOfferCalculatorService = nonOfferCalculatorService;

            Products = new List<Product>();
        }

        public void AddProduct(int productId, int quantity)
        {
            var product = _productRepository.GetProduct(productId);

            for (var i = 0; i < quantity; i++)
            {
                Products.Add(product.ShallowCopy());
            }
        }

        // TODO Put in unit tests for this method
        public decimal CalculateTotalCost()
        {
            decimal totalCost = 0m;

            _bulkBuyOfferCalculatorService.ProcessFreeProducts(Products);
            totalCost += _discountOfferCalculatorService.Calculate(Products);
            totalCost += _nonOfferCalculatorService.Calculate(Products);

            return totalCost;
        }
    }
}