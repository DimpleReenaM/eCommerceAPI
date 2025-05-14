using System.Text.Json.Serialization;

namespace server.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public string ImageNameExt { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
        [JsonIgnore]
        public Brand Brand { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
    }
}
