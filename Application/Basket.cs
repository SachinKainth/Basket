using Domain.Interface.Service;

namespace Application
{
    public class Basket
    {
        private readonly IBasketService _basketService;

        public Basket(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public void AddProduct(int productId, int quantity)
        {
            _basketService.AddProduct(productId, quantity);
        }

        public decimal CalculateTotalCost()
        {
            return _basketService.CalculateTotalCost();
        }
    }
}