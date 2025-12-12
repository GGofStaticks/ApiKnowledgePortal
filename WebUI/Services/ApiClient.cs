using ApiKnowledgePortal.Application.ApiSpec.Commands;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Application.SwaggerParser.Commands;
using ApiKnowledgePortal.Application.SwaggerParser.Dtos;
using ApiKnowledgePortal.Application.SwaggerSources.Commands;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;

namespace WebUI.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;
        public ApiClient(HttpClient http)
        {
            _http = http;
        }

        public Task<PagedResult<ApiSpecDto>> GetApiSpecificationsPagedAsync(int page, int pageSize) =>
            _http.GetFromJsonAsync<PagedResult<ApiSpecDto>>(
                $"api/ApiSpecifications/paged?page={page}&pageSize={pageSize}")!;

        public async Task<ApiSpecDto> CreateApiSpecificationAsync(CreateApiSpecCommand command)
        {
            var response = await _http.PostAsJsonAsync("api/ApiSpecifications", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ApiSpecDto>()!;
        }

        public async Task<ApiSpecDto> UpdateApiSpecificationAsync(UpdateApiSpecCommand command)
        {
            var response = await _http.PutAsJsonAsync($"api/ApiSpecifications/{command.Id}", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ApiSpecDto>()!;
        }

        public async Task DeleteApiSpecificationAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"api/ApiSpecifications/{id}");
            response.EnsureSuccessStatusCode();
        }

        public Task<PagedResult<ParsedApiSpecDto>> GetParsedApiSpecsPagedAsync(int page, int pageSize) =>
            _http.GetFromJsonAsync<PagedResult<ParsedApiSpecDto>>(
                $"api/ParsedApiSpecs/paged?page={page}&pageSize={pageSize}")!;

        public async Task<ParsedApiSpecDto> CreateParsedApiSpecAsync(ParsedApiSpecDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/ParsedApiSpecs", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ParsedApiSpecDto>()!;
        }

        public async Task<ParsedApiSpecDto> UpdateParsedApiSpecAsync(UpdateParsedApiSpecCommand command)
        {
            var response = await _http.PutAsJsonAsync($"api/ParsedApiSpecs/{command.Id}", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ParsedApiSpecDto>()!;
        }

        public async Task DeleteParsedApiSpecAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"api/ParsedApiSpecs/{id}");
            response.EnsureSuccessStatusCode();
        }

        public Task<PagedResult<SwaggerSourceDto>> GetSwaggerSourcesPagedAsync(int page, int pageSize) =>
            _http.GetFromJsonAsync<PagedResult<SwaggerSourceDto>>(
                $"api/SwaggerSources/paged?page={page}&pageSize={pageSize}")!;

        public async Task<SwaggerSourceDto> CreateSwaggerSourceAsync(CreateSwaggerSourceCommand command)
        {
            var response = await _http.PostAsJsonAsync("api/SwaggerSources", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SwaggerSourceDto>()!;
        }

        public async Task<SwaggerSourceDto> UpdateSwaggerSourceAsync(UpdateSwaggerSourceCommand command)
        {
            var response = await _http.PutAsJsonAsync($"api/SwaggerSources/{command.Id}", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SwaggerSourceDto>()!;
        }

        public async Task DeleteSwaggerSourceAsync(Guid id)
        {
            var response = await _http.DeleteAsync($"api/SwaggerSources/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<SwaggerSourceDto>> GetActiveSwaggerSourcesAsync()
        {
            return await _http.GetFromJsonAsync<List<SwaggerSourceDto>>("api/swaggersources/active") ?? new List<SwaggerSourceDto>();
        }

        public async Task FetchOneAsync(Guid sourceId)
        {
            var response = await _http.PostAsync($"api/swaggersources/fetch/{sourceId}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task FetchAllAsync()
        {
            var response = await _http.PostAsync("api/swaggersources/fetch-all", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
