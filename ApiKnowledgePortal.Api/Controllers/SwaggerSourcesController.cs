using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Application.SwaggerSources.Commands;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;
using ApiKnowledgePortal.Application.SwaggerSources.Queries;
using ApiKnowledgePortal.SyncWorker.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiKnowledgePortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SwaggerSourcesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SwaggerSourcesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("paged")]
        public async Task<PagedResult<SwaggerSourceDto>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return await _mediator.Send(new GetSwaggerSourcesPagedQuery(page, pageSize));
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

        [HttpPost("fetch/{id}")]
        public async Task<IActionResult> FetchOne(Guid id)
        {
            var fetcher = HttpContext.RequestServices.GetRequiredService<SwaggerFetcherService>();
            await fetcher.FetchOneAsync(id);
            return Ok(new { Message = "Fetch и парсинг одного источника запущен" });
        }

        [HttpPost("fetch-all")]
        public async Task<IActionResult> FetchAll()
        {
            var fetcher = HttpContext.RequestServices.GetRequiredService<SwaggerFetcherService>();
            await fetcher.FetchAllAsync();
            return Ok(new { Message = "Fetch и парсинг всех активных источников запущен" });
        }

        [HttpGet("active")]
        public async Task<IEnumerable<SwaggerSourceDto>> GetActive()
        {
            return await _mediator.Send(new GetActiveSwaggerSourcesQuery());
        }
    }
}