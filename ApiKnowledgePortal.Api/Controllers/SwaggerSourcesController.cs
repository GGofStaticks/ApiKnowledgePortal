using ApiKnowledgePortal.Application.SwaggerSources.Commands;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;
using ApiKnowledgePortal.Application.SwaggerSources.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiKnowledgePortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SwaggerSourcesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SwaggerSourcesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<SwaggerSourceDto>> GetAll()
        {
            return await _mediator.Send(new GetAllSwaggerSourcesQuery());
        }

        [HttpGet("{id}")]
        public async Task<SwaggerSourceDto> GetById(Guid id)
        {
            return await _mediator.Send(new GetSwaggerSourceByIdQuery(id));
        }

        [HttpPost]
        public async Task<SwaggerSourceDto> Create([FromBody] CreateSwaggerSourceCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<SwaggerSourceDto> Update(Guid id, [FromBody] UpdateSwaggerSourceCommand command)
        {
            return await _mediator.Send(new UpdateSwaggerSourceCommand(id, command.Name, command.Url, command.IsActive));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteSwaggerSourceCommand(id));
            return NoContent();
        }
    }
}