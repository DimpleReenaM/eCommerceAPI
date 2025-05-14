using server.Entities;
using server.Interface.Repository;
using server.Interface.Service;

namespace server.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartItemRepository cartItemRepository;
        private readonly IProductRepository productRepository;

        public CartService(
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IProductRepository productRepository
        )
        {
            this.cartRepository = cartRepository;
            this.cartItemRepository = cartItemRepository;
            this.productRepository = productRepository;
        }

        public async Task AddItemToCart(int userId, int productId, int quantity)
        {
            ShoppingCart cart = await cartRepository.FindCartByUserId(userId);
            if (cart == null)
            {
                cart = new ShoppingCart()
                {
                    UserId = userId,
                    ShoppingCartItems = new List<ShoppingCartItem>()
                };
                await cartRepository.AddAsync(cart);
            }

            var productExists = await productRepository.GetByIdAsync(productId);

            if (productExists is null)
            {
                throw new Exception("Product not found.");
            }
            List<ShoppingCartItem> cartItems = await cartItemRepository.GetAllByCartId(cart.Id);

            ShoppingCartItem item = cartItems.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
            {
                item = new ShoppingCartItem()
                {
                    ShoppingCartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                await cartItemRepository.AddAsync(item);

            }
            else
            {
                item.Quantity += quantity;
                await cartItemRepository.UpdateAsync(item);
            }
        }

        public async Task DeleteCart(int cartId)
        {
            List<ShoppingCartItem> cartItems = await cartItemRepository.GetAllByCartId(cartId);
            foreach (var item in cartItems)
            {
                await cartItemRepository.DeleteAsync(item);
            }

            // ShoppingCart cart = await cartRepository.GetByIdAsync(cartId);
            // await cartRepository.DeleteAsync(cart);

        }

        public Task<ShoppingCartItem?> FindCartItemById(int cartItemId)
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingCart?> FindUserCart(int userId)
        {
            var cart = await cartRepository.FindCartByUserId(userId);
            if (cart == null)
            {
                throw new Exception("No Cart Found");
            }
            List<ShoppingCartItem> cartItems = await cartItemRepository.GetAllByCartId(cart.Id);
            cart.ShoppingCartItems = cartItems;
            return cart;
        }

        public async Task RemoveCartItem(int userId, int cartItemId)
        {
            ShoppingCartItem shoppingCartItem = await cartItemRepository.GetByIdAsync(cartItemId)
            ?? throw new Exception("Item not found.");

            ShoppingCart cart = await cartRepository.GetByIdAsync(shoppingCartItem.ShoppingCartId)
            ?? throw new Exception("Error Cart not found");
            if (cart.UserId != userId)
            {
                throw new Exception("you can't remove anothor users item");
            }
            await cartItemRepository.DeleteAsync(shoppingCartItem);
        }

        public async Task UpdateCartItem(int userId, int cartItemId, int quantity)
        {
             ShoppingCartItem shoppingCartItem = await cartItemRepository.GetByIdAsync(cartItemId)
            ?? throw new Exception("Item not found.");

            ShoppingCart cart = await cartRepository.GetByIdAsync(shoppingCartItem.ShoppingCartId)
            ?? throw new Exception("Error Cart not found");
            if (cart.UserId != userId)
            {
                throw new Exception("You can't update  another users cart item");
            }
            if(quantity<=0){
                await cartItemRepository.DeleteAsync(shoppingCartItem);
                return;
            }
            shoppingCartItem.Quantity = quantity;
            await cartItemRepository.UpdateAsync(shoppingCartItem);

        }
    }
}