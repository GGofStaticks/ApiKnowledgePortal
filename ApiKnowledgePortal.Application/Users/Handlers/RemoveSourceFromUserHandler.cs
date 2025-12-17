using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.Users.Commands;
using ApiKnowledgePortal.Application.Users.Dtos;
using AutoMapper;
using MediatR;

namespace ApiKnowledgePortal.Application.Users.Handlers
{
    public class RemoveSourceFromUserHandler : IRequestHandler<RemoveSourceFromUserCommand, UserDto>
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RemoveSourceFromUserHandler(IUserRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(RemoveSourceFromUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
            {
                throw new InvalidOperationException("Пользователь не найден");
            }

            user.RemoveSource(request.SourceId);
            await _repository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<UserDto>(user);
        }
    }
}
