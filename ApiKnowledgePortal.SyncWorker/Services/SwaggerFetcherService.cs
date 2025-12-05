using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Application.SwaggerParser.Service;
using ApiKnowledgePortal.Domain.ApiSpecifications;
using ApiKnowledgePortal.Domain.ValueObjects;

namespace ApiKnowledgePortal.SyncWorker.Services
{
    public class SwaggerFetcherService
    {
        private readonly ISwaggerSourceRepository _sourceRepository;
        private readonly IApiSpecRepository _apiSpecRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SwaggerParserService _parser;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SwaggerFetcherService> _logger;

        public SwaggerFetcherService(
            ISwaggerSourceRepository sourceRepository,
            IApiSpecRepository apiSpecRepository,
            IUnitOfWork unitOfWork,
            SwaggerParserService parser,
            HttpClient httpClient,
            ILogger<SwaggerFetcherService> logger)
        {
            _sourceRepository = sourceRepository;
            _apiSpecRepository = apiSpecRepository;
            _unitOfWork = unitOfWork;
            _parser = parser;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task FetchAllAsync(CancellationToken cancellationToken = default)
        {
            var sources = await _sourceRepository.GetAllActiveAsync(cancellationToken);

            foreach (var source in sources)
            {
                if (cancellationToken.IsCancellationRequested) return;

                try
                {
                    using var resp = await _httpClient.GetAsync(source.Url, cancellationToken);
                    resp.EnsureSuccessStatusCode();

                    var json = await resp.Content.ReadAsStringAsync(cancellationToken);

                    // попытка извлечь версию из json.info.version
                    string version = "unknown";
                    try
                    {
                        using var doc = JsonDocument.Parse(json);
                        if (doc.RootElement.TryGetProperty("info", out var infoEl) &&
                            infoEl.TryGetProperty("version", out var verEl))
                        {
                            version = verEl.GetString() ?? "unknown";
                        }
                    }
                    catch (JsonException)
                    {
                        // невалидный жсон
                    }

                    // создание аписпецифекейшена новая запись
                    var apiSpec = new ApiSpecifications(
                        ApiSpecId.NewId(),
                        source.Name,
                        version,
                        json
                    );

                    await _apiSpecRepository.AddAsync(apiSpec, cancellationToken);

                    // сохранение изменений
                    source.MarkFetched("Success");
                    await _sourceRepository.UpdateAsync(source, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await _parser.ParseAndSaveAsync(apiSpec);
                }
                catch (Exception ex)
                {
                    source.MarkFetched($"Error: {ex.Message}");
                    await _sourceRepository.UpdateAsync(source, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}