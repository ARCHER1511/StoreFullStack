namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public required string Email { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
