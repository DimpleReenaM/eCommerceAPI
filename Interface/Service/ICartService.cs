using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Entities;

namespace server.Interface.Service
{
    public interface ICartService
    {
        Task<ShoppingCart?> FindUserCart(int userId);
        Task AddItemToCart(int userId,int productId,int quantity);
        Task DeleteCart(int cartId);

        Task<ShoppingCartItem?> FindCartItemById(int cartItemId);
        Task RemoveCartItem(int userId,int cartItemId);
        Task UpdateCartItem(int userId,int cartItemId,int quantity);
    }
}