namespace Core.DTOs
{
    public class BaseDto
    {
        // API uygulamalarında DTO. MVC projelerinde View Model ismini veriyoruz
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
