namespace server.Entities
{
    public class ShippingAddress
    {
        public int Id { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set;}

        // Street address
        public string AddressLine1 { get; set; }

        // Optional additional address line (e.g., apartment or suite number)
        public string? AddressLine2 { get; set; }

        // City
        public string City { get; set; }

        // State or province
        public string State { get; set; }

        // Postal code or ZIP code
        public string PostalCode { get; set; }

        // Country
        public string Country { get; set; }  
        public int OrderId { get; set; }
    }
}