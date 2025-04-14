using System.ComponentModel.DataAnnotations;

namespace API.DAL.Entities
{
    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }
        public string? RoleName { get; set; }

    }
}
