using server.Entities;

namespace server.Interface.Repository
{
    public interface ICartItemRepository:IGenericRepository<ShoppingCartItem>
    {
        Task<List<ShoppingCartItem>> GetAllByCartId(int cartId);
    }
}