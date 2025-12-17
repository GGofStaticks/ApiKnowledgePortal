using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.Users.Commands;
using ApiKnowledgePortal.Application.Users.Dtos;
using ApiKnowledgePortal.Domain.Users;
using AutoMapper;
using MediatR;

namespace ApiKnowledgePortal.Application.Users.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserDto>
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterUserHandler(IUserRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _repository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Пользователь с такой почтой уже существует");
            }

            var user = User.Create(request.FirstName, request.LastName, request.Email, request.Password);
            await _repository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<UserDto>(user);
        }
    }
}
