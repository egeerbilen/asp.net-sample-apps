using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class ProductDto : BaseDto
    {
        // Eskiden 
        // [Required(ErrorMessage = "Hata")] -> şeklinde buraya yazılırdı bu uygun bir yöntem değil yönetimi zor olur
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
