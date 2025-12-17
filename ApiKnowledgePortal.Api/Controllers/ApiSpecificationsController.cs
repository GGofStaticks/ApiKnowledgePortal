using ApiKnowledgePortal.Application.ApiSpec.Commands;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using ApiKnowledgePortal.Application.ApiSpec.Queries;
using ApiKnowledgePortal.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiKnowledgePortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApiSpecificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApiSpecificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("paged")]
        public async Task<PagedResult<ApiSpecDto>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return await _mediator.Send(new GetApiSpecPagedQuery(page, pageSize));
        }

        [HttpGet]
        public async Task<IEnumerable<ApiSpecDto>> GetAll()
        {
            return await _mediator.Send(new GetAllApiSpecsQuery());
        }

        [HttpGet("{id}")]
        public async Task<ApiSpecDto> GetById(Guid id)
        {
            return await _mediator.Send(new GetApiSpecByIdQuery(id));
        }

        [HttpPost]
        public async Task<ApiSpecDto> Create([FromBody] CreateApiSpecCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ApiSpecDto> Update(Guid id, [FromBody] UpdateApiSpecCommand command)
        {
            return await _mediator.Send(new UpdateApiSpecCommand(id, command.Content));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteApiSpecCommand(id));
            return NoContent();
        }
    }
}
