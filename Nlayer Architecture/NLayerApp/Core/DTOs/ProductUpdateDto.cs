using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ProductUpdateDto
    {
        // Bu bir custom DTO dur update ederken bunları istiyoruz
        // örnekolsun diye var update yaparken bu dto yu kullanacak
        // artık kullanaılacak DTO lar projeye göre çoğaltıla bilir
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
