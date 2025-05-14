namespace server.Dto.Auth
{
    public class RegisterSellerDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; }


        public string? BusinessName { get; set; }


        public string? BusinessType { get; set; }


        public string? GSTNumber { get; set; }
    }
}
