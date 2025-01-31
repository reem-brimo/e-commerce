namespace E_Commerce.SharedKernal.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public interface IBaseEntity
    {
        public bool IsDeleted { get; set; }
    }
}