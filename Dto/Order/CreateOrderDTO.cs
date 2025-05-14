namespace server.Dto
{
    public class CreateOrderDTO
    {
        public int CartId { get; set; }
        public AddressDto ShipToAddress { get; set; }
    }
}