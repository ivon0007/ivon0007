namespace WindowsFormsApp10.Models
{
    public enum UserRole { Admin, User }

    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}