namespace DomainLayer.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
