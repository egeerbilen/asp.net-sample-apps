using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class Product : BaseEntity
    {
        // ctor -> constroctor

        // [Required] -> bunu configuration dosyasında vereceksin burada vermek yerine
        public string Name { get; set; }
        // public required string Name { get; set; }
        // public string? Name { get; set; } -> ? işareti konuğunda null olabilir demek
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ProductFeature ProductFeature { get; set; }
    }
}
