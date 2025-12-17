using ApiKnowledgePortal.Application.ApiSpec.Commands;
using ApiKnowledgePortal.Application.ApiSpec.Dtos;
using ApiKnowledgePortal.Application.Common;
using ApiKnowledgePortal.Application.SwaggerParser.Commands;
using ApiKnowledgePortal.Application.SwaggerParser.Dtos;
using ApiKnowledgePortal.Application.SwaggerSources.Commands;
using ApiKnowledgePortal.Application.SwaggerSources.Dtos;
using ApiKnowledgePortal.Application.Users.Commands;
using ApiKnowledgePortal.Application.Users.Dtos;
using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace WebUI.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public ApiClient(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
            _http.DefaultRequestHeaders.Accept.Clear();
            _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task SetBearerToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<PagedResult<ApiSpecDto>> GetApiSpecificationsPagedAsync(int page, int pageSize)
        {
            await SetBearerToken();
            return await _http.GetFromJsonAsync<PagedResult<ApiSpecDto>>(
                $"api/ApiSpecifications/paged?page={page}&pageSize={pageSize}")!;
        }

        public async Task<ApiSpecDto> CreateApiSpecificationAsync(CreateApiSpecCommand command)
        {
            await SetBearerToken();
            var response = await _http.PostAsJsonAsync("api/ApiSpecifications", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ApiSpecDto>()!;
        }
        public async Task<ApiSpecDto> UpdateApiSpecificationAsync(UpdateApiSpecCommand command)
        {
            await SetBearerToken();
            var response = await _http.PutAsJsonAsync($"api/ApiSpecifications/{command.Id}", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ApiSpecDto>()!;
        }
        public async Task DeleteApiSpecificationAsync(Guid id)
        {
            await SetBearerToken();
            var response = await _http.DeleteAsync($"api/ApiSpecifications/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task<PagedResult<ParsedApiSpecDto>> GetParsedApiSpecsPagedAsync(int page, int pageSize)
        {
            await SetBearerToken();
            return await _http.GetFromJsonAsync<PagedResult<ParsedApiSpecDto>>(
                $"api/ParsedApiSpecs/paged?page={page}&pageSize={pageSize}")!;
        }
        public async Task<ParsedApiSpecDto> CreateParsedApiSpecAsync(ParsedApiSpecDto dto)
        {
            await SetBearerToken();
            var response = await _http.PostAsJsonAsync("api/ParsedApiSpecs", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ParsedApiSpecDto>()!;
        }
        public async Task<ParsedApiSpecDto> UpdateParsedApiSpecAsync(UpdateParsedApiSpecCommand command)
        {
            await SetBearerToken();
            var response = await _http.PutAsJsonAsync($"api/ParsedApiSpecs/{command.Id}", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ParsedApiSpecDto>()!;
        }
        public async Task DeleteParsedApiSpecAsync(Guid id)
        {
            await SetBearerToken();
            var response = await _http.DeleteAsync($"api/ParsedApiSpecs/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task<PagedResult<SwaggerSourceDto>> GetSwaggerSourcesPagedAsync(int page, int pageSize)
        {
            await SetBearerToken();
            return await _http.GetFromJsonAsync<PagedResult<SwaggerSourceDto>>(
                $"api/SwaggerSources/paged?page={page}&pageSize={pageSize}")!;
        }
        public async Task<SwaggerSourceDto> CreateSwaggerSourceAsync(CreateSwaggerSourceCommand command)
        {
            await SetBearerToken();
            var response = await _http.PostAsJsonAsync("api/SwaggerSources", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SwaggerSourceDto>()!;
        }
        public async Task<SwaggerSourceDto> UpdateSwaggerSourceAsync(UpdateSwaggerSourceCommand command)
        {
            await SetBearerToken();
            var response = await _http.PutAsJsonAsync($"api/SwaggerSources/{command.Id}", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SwaggerSourceDto>()!;
        }
        public async Task DeleteSwaggerSourceAsync(Guid id)
        {
            await SetBearerToken();
            var response = await _http.DeleteAsync($"api/SwaggerSources/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task<List<SwaggerSourceDto>> GetActiveSwaggerSourcesAsync()
        {
            await SetBearerToken();
            return await _http.GetFromJsonAsync<List<SwaggerSourceDto>>("api/swaggersources/active") ?? new List<SwaggerSourceDto>();
        }
        public async Task FetchOneAsync(Guid sourceId)
        {
            await SetBearerToken();
            var response = await _http.PostAsync($"api/swaggersources/fetch/{sourceId}", null);
            response.EnsureSuccessStatusCode();
        }
        public async Task FetchAllAsync()
        {
            await SetBearerToken();
            var response = await _http.PostAsync("api/swaggersources/fetch-all", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<UserDto> RegisterAsync(RegisterUserCommand command)
        {
            var response = await _http.PostAsJsonAsync("api/auth/register", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>()!;
        }

        public async Task<string> LoginAsync(LoginUserCommand command)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", command);
            response.EnsureSuccessStatusCode();
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            return loginResponse!.Token;
        }

        public async Task<PagedResult<UserDto>> GetUsersPagedAsync(int page, int pageSize)
        {
            await SetBearerToken();
            return await _http.GetFromJsonAsync<PagedResult<UserDto>>(
                $"api/users/paged?page={page}&pageSize={pageSize}")!;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            await SetBearerToken();
            return await _http.GetFromJsonAsync<UserDto>($"api/users/{id}")!;
        }

        public async Task<UserDto> UpdateUserRoleAsync(UpdateUserRoleCommand command)
        {
            await SetBearerToken();
            var response = await _http.PutAsJsonAsync("api/users/role", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>()!;
        }

        public async Task<UserDto> AddSourceToUserAsync(AddSourceToUserCommand command)
        {
            await SetBearerToken();
            var response = await _http.PutAsJsonAsync("api/users/add-source", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>()!;
        }

        public async Task<UserDto> RemoveSourceFromUserAsync(RemoveSourceFromUserCommand command)
        {
            await SetBearerToken();
            var response = await _http.PutAsJsonAsync("api/users/remove-source", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>()!;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await SetBearerToken();
            var response = await _http.DeleteAsync($"api/users/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            await SetBearerToken();
            return await _http.GetFromJsonAsync<UserDto>("api/users/current")!;
        }
    }
}