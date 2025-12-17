using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Application.Users.Commands;
using ApiKnowledgePortal.Application.Users.Dtos;
using ApiKnowledgePortal.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiKnowledgePortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // только для админа
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("paged")]
        public async Task<PagedResult<UserDto>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return await _mediator.Send(new GetUsersPagedQuery(page, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<UserDto> GetById(Guid id)
        {
            return await _mediator.Send(new GetUserByIdQuery(id));
        }

        [HttpPut("role")]
        public async Task<UserDto> UpdateRole([FromBody] UpdateUserRoleCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("add-source")]
        public async Task<UserDto> AddSource([FromBody] AddSourceToUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("remove-source")]
        public async Task<UserDto> RemoveSource([FromBody] RemoveSourceFromUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
            return NoContent();
        }

        [HttpGet("current")]
        [AllowAnonymous] // но будет проверка в handler
        public async Task<UserDto> GetCurrent()
        {
            return await _mediator.Send(new GetCurrentUserQuery());
        }
    }
}
