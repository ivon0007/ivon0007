using System.Collections.Generic;
using System.Linq;
using WindowsFormsApp10.Models;

namespace WindowsFormsApp10.Services
{
    public class AuthService
    {
        private List<User> _users;

        public AuthService()
        {
            // Создаем тестовых пользователей
            _users = new List<User>
            {
                new User { Login = "admin", Password = "123", Role = UserRole.Admin },
                new User { Login = "user", Password = "123", Role = UserRole.User }
            };
        }

        public User Authenticate(string login, string password)
        {
            return _users.FirstOrDefault(u => u.Login == login && u.Password == password);
        }
    }
}