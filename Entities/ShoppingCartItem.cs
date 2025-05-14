using System.Text.Json.Serialization;

namespace server.Entities
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        [JsonIgnore]
        public ShoppingCart ShoppingCart { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPriceAfterDiscount
        {
            get
            {
                return Product.NewPrice * Quantity;
            }
        }

        public decimal TotalDiscount
        {
            get
            {
                return (Product.OriginalPrice-Product.NewPrice) * Quantity;
            }
        }

        public decimal TotalPrice
        {
            get
            {
                return Product.OriginalPrice * Quantity;
            }
        }
    }
}
