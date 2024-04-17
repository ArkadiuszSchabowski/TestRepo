namespace _076.AngularExcercisesAPI.Models
{
    public class CreateUserDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string Email { get; set; }
        public int? Phone { get; set; }
        public string? City { get; set; }
        public string Country { get; set; }
    }
}
