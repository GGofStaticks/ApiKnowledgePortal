using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Domain.Users;

namespace ApiKnowledgePortal.Application.Users.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public UserRole Role { get; set; }
        public List<Guid> Sources { get; set; } = new List<Guid>();
    }
}