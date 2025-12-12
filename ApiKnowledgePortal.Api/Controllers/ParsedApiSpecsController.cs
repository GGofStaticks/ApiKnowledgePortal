using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Application.SwaggerParser.Commands;
using ApiKnowledgePortal.Application.SwaggerParser.Dtos;
using ApiKnowledgePortal.Application.SwaggerParser.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiKnowledgePortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParsedApiSpecsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParsedApiSpecsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("paged")]
        public async Task<PagedResult<ParsedApiSpecDto>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return await _mediator.Send(new GetParsedApiSpecsPagedQuery(page, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<ParsedApiSpecDto> GetById(Guid id)
        {
            return await _mediator.Send(new GetParsedApiSpecByIdQuery(id));
        }

        [HttpPost]
        public async Task<ParsedApiSpecDto> Create([FromBody] CreateParsedApiSpecCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ParsedApiSpecDto> Update(Guid id, [FromBody] UpdateParsedApiSpecCommand command)
        {
            // айди из маршрута, чтобы не полагаться на тело запроса
            return await _mediator.Send(new UpdateParsedApiSpecCommand(id, command.Summary));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteParsedApiSpecCommand(id));
            return NoContent();
        }

        [HttpPost("parse/{apiSpecId}")]
        public async Task<IActionResult> Parse(Guid apiSpecId)
        {
            await _mediator.Send(new ParseSwaggerCommand(apiSpecId));
            return Ok(new { Message = "Парсинг запущен" });
        }
    }
}
