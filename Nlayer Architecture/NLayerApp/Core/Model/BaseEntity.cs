namespace Core.Model
{
    public abstract class BaseEntity // new anahtar dözcüğünü nün kullanımını engelledik çünkü bu bizim için bir base yapı
    {
        //prop
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; } // eklendiği esnada null olacağı için ? koyduk
    }
}
