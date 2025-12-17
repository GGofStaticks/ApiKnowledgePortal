using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Domain.Abstractions;
using ApiKnowledgePortal.Domain.Users;
using BCrypt.Net;

namespace ApiKnowledgePortal.Domain.Users
{
    public class User : Entity<Guid>
    {
        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        public UserRole Role { get; private set; }
        public List<Guid> Sources { get; private set; } = new List<Guid>();

        private User() { }

        public User(Guid id, string firstName, string lastName, string email, string password)
        {
            Id = id;
            FirstName = !string.IsNullOrWhiteSpace(firstName) ? firstName : throw new ArgumentException("Имя не может быть пустым", nameof(firstName));
            LastName = !string.IsNullOrWhiteSpace(lastName) ? lastName : throw new ArgumentException("Фамилия не может быть пустой", nameof(lastName));
            Email = !string.IsNullOrWhiteSpace(email) ? email : throw new ArgumentException("Почта не может быть пустой", nameof(email));
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            Role = UserRole.User; // по умолчанию роль обычная
        }

        public static User Create(string firstName, string lastName, string email, string password)
        {
            return new User(Guid.NewGuid(), firstName, lastName, email, password);
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        public void ChangeRole(UserRole newRole)
        {
            Role = newRole;
        }

        public void AddSource(Guid sourceId)
        {
            if (!Sources.Contains(sourceId))
            {
                Sources.Add(sourceId);
            }
        }

        public void RemoveSource(Guid sourceId)
        {
            Sources.Remove(sourceId);
        }
    }
}