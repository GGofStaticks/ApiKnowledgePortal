using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.Users.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ApiKnowledgePortal.Application.Users.Handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _configuration;

        public LoginUserHandler(IUserRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null || !user.VerifyPassword(request.Password))
            {
                throw new UnauthorizedAccessException("Неверная почта или пароль");
            }

            // генерация джвт
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            foreach (var source in user.Sources)
            {
                claims.Add(new Claim("sources", source.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(7);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}