namespace API.DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string? Email { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public UserRole? Role { get; set; }
        public int RoleId { get; set; }     // foreign key (Important)
    }

}
