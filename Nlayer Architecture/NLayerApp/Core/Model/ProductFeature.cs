namespace Core.Model
{
    public class ProductFeature
    {
        // Product ile ilgili ek alanları Product altında tutmak yerine ProductFeature altında tutuyoruz
        public int Id { get; set; }
        public string Color { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int ProductId { get; set; } // ProductId değerini boşluksuz yazdığımız için (Product_Id bu şekilde yazmadık) bunu bir forenkey olarak algılayacak
        public Product Product { get; set; }

    }
}
