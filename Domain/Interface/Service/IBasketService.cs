namespace Domain.Interface.Service
{
    public interface IBasketService
    {
        void AddProduct(int productId, int quantity);
        decimal CalculateTotalCost();
    }
}