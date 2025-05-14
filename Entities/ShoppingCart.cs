namespace server.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        public decimal TotalPriceAfterDiscount
        {
            get
            {
                // Sum the price of all items in the cart
                return ShoppingCartItems.Sum(item => item.TotalPriceAfterDiscount);
            }
        }
        public decimal TotalDiscount
        {
            get
            {
                // Sum the price of all items in the cart
                return ShoppingCartItems.Sum(item => item.TotalDiscount);
            }
        }
        public decimal TotalPrice
        {
            get
            {
                // Sum the price of all items in the cart
                return ShoppingCartItems.Sum(item => item.TotalPrice);
            }
        }
        // Optional: A property to track the total number of items in the cart
        public int TotalItems
        {
            get
            {
                return ShoppingCartItems.Sum(item => item.Quantity);
            }
        }
    }
}
