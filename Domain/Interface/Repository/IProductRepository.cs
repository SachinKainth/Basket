using Domain.Entity;

namespace Domain.Interface.Repository
{
    public interface IProductRepository
    {
        Product GetProduct(int productId);
    }
}