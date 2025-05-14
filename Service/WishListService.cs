using Microsoft.EntityFrameworkCore;
using server.Dto;
using server.Entities;
using server.Exceptions;
using server.Interface.Repository;
using server.Interface.Service;
using server.Repository;

namespace server.Service
{
    public class WishListService:IWishListService
    {
        private readonly IWishListRepository repo;
        private readonly IWishListItemRepository wishListItemRepository;
        private readonly IProductRepository productRepository;

        public WishListService(
            IWishListRepository repo, 
            IWishListItemRepository wishListItemRepository, 
            IProductRepository productRepository
            )
        {
            this.repo = repo;
            this.wishListItemRepository = wishListItemRepository;
            this.productRepository = productRepository;
        }
        public async Task<Wishlist?> GetWishlistIncludeProductAsync(int userId)
        {
            return await repo.GetWishlistByUserIdIncludeProductAsync(userId);
        }

        public async Task AddToWishlistAsync(int userId, int productId)
        {
            var wishlist = await repo.GetWishlistByUserIdAsync(userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    UserId = userId,
                    WishlistItems = new List<WishlistItem>()
                };
                await repo.AddAsync(wishlist);
            }

            var productExists = await productRepository.GetByIdAsync(productId);

            if (productExists is null)
            {
                throw new Exception("Product not found.");
            }

            var itemExists = wishlist.WishlistItems.Any(wi => wi.ProductId == productId);
            if (!itemExists)
            {
                await wishListItemRepository.AddAsync(new WishlistItem
                {
                    ProductId = productId,
                    WishlistId = wishlist.Id
                });  
            }
        }

        public async Task RemoveFromWishlistAsync(int userId, int productId)
        {
            var wishlist = await repo.GetWishlistByUserIdAsync(userId);

            if (wishlist == null)
            {
                throw new NotFoundException("Wishlist not found.");
            }

            var wishlistItem = wishlist.WishlistItems.FirstOrDefault(wi => wi.ProductId == productId);

            if (wishlistItem == null)
            {
                throw new NotFoundException("Product not found in wishlist.");
            }
            await wishListItemRepository.DeleteAsync(wishlistItem);
        }
    }
}
