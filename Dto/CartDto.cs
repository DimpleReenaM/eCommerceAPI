
namespace server.Dto;

public class AddItemToCartRequest
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
public class UpdateCartItemRequest
{
    public int UserId { get; set; }
    public int CartItemId { get; set; }
    public int Quantity { get; set; }
}



 public class ShoppingCartItemResDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductResDto Product { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice{ get; set; }
    }



    public class ShoppingCartResDto
    {
        public int Id { get; set; }
        public ICollection<ShoppingCartItemResDto> ShoppingCartItems { get; set; }
        public decimal TotalPrice{ get; set; }
         public decimal TotalDiscount{ get; set; }
          public decimal TotalPriceAfterDiscount{ get; set; }
        public int TotalItems{ get; set; }
    }