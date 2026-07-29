
namespace XYZ_shop.Domain.Entities
{
    public class UserProfileEntity : BaseEntity
    {
        public string Email { get; set; }        
        public string? FirstName { get; set; }         
        public string? LastName { get; set; }
        public string? Mobilephone { get; set; }
        public DateTime? BirthDate { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
