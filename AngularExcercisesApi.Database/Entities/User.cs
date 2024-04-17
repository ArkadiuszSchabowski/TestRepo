namespace _076.AngularExcercisesAPI.Database.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string HashPassword { get; set; }
        public string Email { get; set; }
        public int? Phone { get; set; }
        public string? City { get; set; }
        public string Country { get; set; }
        public int RoleId { get; set; } = 1;
        public virtual Role Role { get; set; }
    }
}
