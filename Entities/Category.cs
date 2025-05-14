namespace server.Entities
{
    public class Category: AuditBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? ImageId { get; set; }
        public Image? Image { get; set; }
    }
}
