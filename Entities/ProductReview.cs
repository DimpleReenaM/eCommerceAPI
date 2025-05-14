namespace server.Entities
{
    public class ProductReview: AuditBaseEntity
    {
        public int Id { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
         public int UserId { get; set; }
        public User User { get; set; }

    }
}
